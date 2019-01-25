using ComponentsLoader;
using LegionContract;
using LegionCore.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LegionCore.Architecture.Server
{
    internal class ServerTask : IDisposable
    {
        private LoadedComponent<LegionTask> _Task;
        private ParametersManager _ParametersManager;

        private string _TaskOutputPath;
        private StreamWriter _DataOutWriter;
        private string _OrderedOutputPath;
        private List<LegionDataOut> _DataOut;
        private bool _WasSavedWithOrder;

        private LoggingManager _Logger;

        private List<Tuple<LegionDataIn, DateTime>> _TaskWithTimeout;
        private long _TimeoutTime;

        private int _Id;

        public LoadedComponent<LegionTask> Task { get => _Task; }
        public bool NoParametersAvailable { get => _ParametersManager.NoParametersAvailable; }

        public bool TaskClosed { get; private set; }

        public ServerTask(LoadedComponent<LegionTask> task,
            List<string> taskInputPaths,
            string taskOutputPath,
            string taskOutputOrderedPath,
            long timeoutTime,
            int id)
        {
            _Logger = LoggingManager.Instance;
            _ParametersManager = new ParametersManager(taskInputPaths);
            _Task = task;
            _TaskOutputPath = taskOutputPath;
            _OrderedOutputPath = taskOutputOrderedPath;
            _Id = id;

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
                            _Logger.LogWarning($"[ Server ] Timeout detected " +
                                $"taskId: {_Id}, " +
                                $"parameter with id: {IdManagement.GetId(item.Item1)}");


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
            if (_TimeoutTime <= 0 || dataIn == null)
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

            lock (_ParametersManager)
            {
                lock (_DataOutWriter)
                {
                    if (TaskClosed)
                        return true;
                    _ParametersManager.CheckNextInputParameters();
                    TaskClosed = _ParametersManager.NoParametersAvailable &&
                        _DataOut.Count == _ParametersManager.LastParameterId;
                    return TaskClosed;
                }
            }
        }

        private void InitStreams()
        {
            if (_Task != null &&
                !string.IsNullOrEmpty(_TaskOutputPath))
            {
                _ParametersManager.Init();
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

        internal LegionDataIn GetDataIn()
        {
            lock (_ParametersManager)
            {
                return GetTimeoutDataIn() ?? GetNormalDataIn();
            }
        }

        private LegionDataIn GetNormalDataIn()
        {
            LegionDataIn dataIn = _ParametersManager.GetNormalDataIn(DataIn);
            if (!(dataIn is LegionErrorDataIn))
                AddTaskToTimeoutList(dataIn);
            return dataIn;
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
                    _Logger.LogWarning("[ Server ] Timeouted task reinitialized! " +
                        $"task id: {_Id}, " +
                        $"parameter id: {IdManagement.GetId(dataIn)}");
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
                if (NoParametersAvailable && _DataOut?.Count == _ParametersManager.LastParameterId && !_WasSavedWithOrder)
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
            _ParametersManager?.Dispose();
            _DataOutWriter?.Dispose();
        }
    }

}
