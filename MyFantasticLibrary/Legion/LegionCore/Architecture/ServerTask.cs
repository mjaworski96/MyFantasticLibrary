using ComponentsLoader;
using LegionContract;
using LegionCore.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LegionCore.Architecture
{
    internal class ServerTask : IDisposable
    {
        private LoadedComponent<LegionTask> _Task;

        private List<string> _TaskInputPaths;
        private string _TaskOutputPath;
        private StreamReader _DataInReader;
        private StreamWriter _DataOutWriter;
        private string _OrderedOutputPath;
        private List<LegionDataOut> _DataOut;
        private bool _WasSavedWithOrder;

        private int _CurrentTaskParameter;
        private int _LastParamId;
        private LoggingManager _Logger;

        private List<Tuple<LegionDataIn, DateTime>> _TaskWithTimeout;
        private long _TimeoutTime;

        public LoadedComponent<LegionTask> Task { get => _Task; }
        public bool NoParametersAvailable { get; private set; }

        public bool TaskClosed { get; private set; }

        public ServerTask(LoadedComponent<LegionTask> task,
            List<string> taskInputPaths,
            string taskOutputPath,
            string taskOutputOrderedPath,
            long timeoutTime)
        {
            _Logger = LoggingManager.Instance;
            _TaskInputPaths = new List<string>();
            _Task = task;
            _TaskInputPaths = taskInputPaths;
            _TaskOutputPath = taskOutputPath;
            _CurrentTaskParameter = _LastParamId = 0;
            NoParametersAvailable = false;
            _OrderedOutputPath = taskOutputOrderedPath;

            _TaskWithTimeout = new List<Tuple<LegionDataIn, DateTime>>();
            if (timeoutTime > 0)
                _TimeoutTime = timeoutTime;

            if (_OrderedOutputPath != null)
                _DataOut = new List<LegionDataOut>();
            InitStreams();
        }

        internal bool HasAnyTimeouts
        {
            get
            {
                DateTime now = DateTime.Now;
                lock (_TaskWithTimeout)
                {
                    foreach (var item in _TaskWithTimeout)
                    {
                        if (item.Item2 < now)
                        {
                            _Logger.LogWarning("Timeout detected for parameter with id: " + IdManagement.GetId(item.Item1));
                            return true;
                        }
                            
                    }
                }
                return false;
            }
        }
        internal LegionDataIn TimeoutedTask
        {
            get
            {
                DateTime now = DateTime.Now;
                lock (_TaskWithTimeout)
                {
                    foreach (var item in _TaskWithTimeout)
                    {
                        if (item.Item2 < now)
                            return item.Item1;
                    }
                }
                return null;
            }
        }
        internal void RemoveTaskFromTimeoutList(int taskId)
        {
            lock (_TaskWithTimeout)
            {
                _TaskWithTimeout.RemoveAll(x => IdManagement.GetId(x.Item1) == taskId);
            }
        }
        internal void AddTaskToTimeoutList(LegionDataIn dataIn)
        {
            if (_TimeoutTime <= 0)
                return;
            lock (_TaskWithTimeout)
            {
                DateTime timeoutTime = GetTimeoutTime();
                _TaskWithTimeout.Add(Tuple.Create(dataIn, timeoutTime));
            }
        }

        private DateTime GetTimeoutTime()
        {
            return DateTime.Now.AddMilliseconds(_TimeoutTime);
        }

        internal bool WasTaskEnded(int id)
        {
            return _DataOut.Count(x => IdManagement.GetId(x) == id) != 0;
        }
        internal bool CheckIfFinish()
        {
            if (Error)
                return true;

            lock (_DataInReader)
            {
                lock (_DataOutWriter)
                {
                    if (TaskClosed)
                        return true;
                    CheckNextInputParameters();
                    if (_DataInReader.EndOfStream && _DataOut.Count == _LastParamId)
                    {
                        TaskClosed = true;
                        return true;
                    }                   
                    else
                        return false;
                }
            }
        }

        private void InitStreams()
        {
            if (_Task != null && _TaskInputPaths.Count > 0
                && _TaskInputPaths.Count > 0 &&
                !string.IsNullOrEmpty(_TaskOutputPath))
            {
                _DataInReader = new StreamReader(_TaskInputPaths[0]);
                _DataOutWriter = new StreamWriter(_TaskOutputPath);

            }
        }

        internal LegionDataIn DataIn
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)_Task.Singleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataIn)Activator.CreateInstance(attr.TypeIn);
            }
        }
        internal LegionDataOut DataOut
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)_Task.Singleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataOut)Activator.CreateInstance(attr.TypeOut);
            }
        }

        public bool Error { get; internal set; }

        internal void CheckNextInputParameters()
        {
            if (!TaskClosed && _DataInReader.EndOfStream)
            {
                if (_CurrentTaskParameter + 1 < _TaskInputPaths.Count)
                {
                    _CurrentTaskParameter++;
                    _DataInReader.Close();
                    _DataInReader = new StreamReader(
                       _TaskInputPaths[_CurrentTaskParameter]);
                }
                else
                {
                    NoParametersAvailable = true;
                }
            }

        }

        internal LegionDataIn GetDataIn()
        {
            lock (_DataInReader)
            {
                return GetTimeoutDataIn() ?? GetNormalDataIn();
            }
        }

        private LegionDataIn GetNormalDataIn()
        {
            CheckNextInputParameters();
            if (!TaskClosed && !_DataInReader.EndOfStream)
            {
                LegionDataIn dataIn = DataIn;
                AddTaskToTimeoutList(dataIn);
                IdManagement.SetId(dataIn, _LastParamId);
                _LastParamId++;
                dataIn.LoadFromStream(_DataInReader);
                return dataIn;
            }

            return null;
        }
        private LegionDataIn GetTimeoutDataIn()
        {
            if (HasAnyTimeouts)
            {
                LegionDataIn dataIn = TimeoutedTask;
                if (dataIn != null)
                {
                    RemoveTaskFromTimeoutList(IdManagement.GetId(dataIn));
                    AddTaskToTimeoutList(dataIn);
                    _Logger.LogWarning("Timeouted task reinitialized! Parameter id: " + IdManagement.GetId(dataIn));
                    return dataIn;
                }
                    
            }
            return null;
        }

        internal void SaveResults(IEnumerable<LegionDataOut> dataOut)
        {
            lock (_DataOutWriter)
            {
                foreach (var data in dataOut)
                {
                    int id = IdManagement.GetId(data);
                    if (WasTaskEnded(id))
                        return;

                    RemoveTaskFromTimeoutList(id);
                    FlushDataOut(data, id);
                }
                if (NoParametersAvailable && _DataOut?.Count == _LastParamId && !_WasSavedWithOrder)
                    SaveResultsWithOrder();
            }
        }

        private void FlushDataOut(LegionDataOut data, int id)
        {
            _DataOutWriter.Write(id + " -> ");
            data.SaveToStream(_DataOutWriter);

            _DataOutWriter.Flush();

            if (_DataOut != null)
                _DataOut.Add(data);
        }

        private void SaveResultsWithOrder()
        {
            lock (_OrderedOutputPath)
            {
                using (StreamWriter ordered = new StreamWriter(_OrderedOutputPath))
                {
                    foreach (var data in _DataOut.OrderBy(x => IdManagement.GetId(x)))
                    {
                        data.SaveToStream(ordered);
                    }
                }
                _Logger.LogInformation("[ Server ] Saved output data with order.");
                _WasSavedWithOrder = true;
            }
        }
        public void Dispose()
        {
            _DataInReader?.Dispose();
            _DataOutWriter?.Dispose();
        }
    }

}
