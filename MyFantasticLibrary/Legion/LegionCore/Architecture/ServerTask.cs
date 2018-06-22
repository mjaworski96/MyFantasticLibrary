using ComponentsLoader;
using LegionContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LegionCore.Architecture
{
    internal class ServerTask: IDisposable
    {
        private LoadedComponent<LegionTask> _Task;
        private List<string> _TaskInputPaths = new List<string>();
        private string _TaskOutputPath;
        private int _CurrentTaskParameter;
        private StreamReader dataInReader;
        private StreamWriter dataOutWriter;

        public ServerTask(LoadedComponent<LegionTask> task, 
            List<string> taskInputPaths, 
            string taskOutputPath)
        {
            _Task = task;
            _TaskInputPaths = taskInputPaths;
            _TaskOutputPath = taskOutputPath;
            _CurrentTaskParameter = 0;
            InitStreams();
        }

        private void InitStreams()
        {
            if (_Task != null && _TaskInputPaths.Count > 0
                && _TaskInputPaths.Count > 0 &&
                !string.IsNullOrEmpty(_TaskOutputPath))
            {
                dataInReader = new StreamReader(_TaskInputPaths[0]);
                dataOutWriter = new StreamWriter(_TaskOutputPath);

            }
        }

        internal LegionDataIn CurrentDataIn
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)_Task.Singleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataIn)Activator.CreateInstance(attr.TypeIn);
            }
        }
        internal LegionDataOut CurrentDataOut
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)_Task.Singleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataOut)Activator.CreateInstance(attr.TypeOut);
            }
        }

        public LoadedComponent<LegionTask> Task { get => _Task; }

        internal void CheckNextInputParameters()
        {
            if (dataInReader.EndOfStream &&
                _CurrentTaskParameter + 1 < _TaskInputPaths.Count)
            {
                _CurrentTaskParameter++;
                dataInReader = new StreamReader(
                   _TaskInputPaths[_CurrentTaskParameter]);
            }
        }

        internal LegionDataIn GetDataIn()
        {
            lock (dataInReader)
            {
                CheckNextInputParameters();
                if(!dataInReader.EndOfStream)
                {
                    LegionDataIn dataIn = CurrentDataIn;
                    dataIn.LoadFromStream(dataInReader);
                    return dataIn;
                }

                return null;
            }
        }

        internal void SaveResults(IEnumerable<LegionDataOut> dataOut)
        {
            lock (dataOutWriter)
            {
                foreach (var data in dataOut)
                {
                    data.SaveToStream(dataOutWriter);
                }
            }
        }

        public void Dispose()
        {
            dataInReader?.Dispose();
            dataOutWriter?.Dispose();
        }
    }

}
