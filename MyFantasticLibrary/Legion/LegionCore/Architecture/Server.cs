using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ComponentsLoader;
using ConfigurationManager;
using LegionContract;

namespace LegionCore.Architecture
{
    public class Server: IDisposable
    {
        List<LoadedComponent<LegionTask>> _Tasks = new List<LoadedComponent<LegionTask>>();
        int _CurrentTask;
        List<List<string>> _TasksInputParameters = new List<List<string>>();
        List<string> _TasksOutputParameters = new List<string>();
        int _CurrentTaskParameter;
        //TODO: List<Tuple<LegionDataIn, int>> _TasksTimeoutCount = new List<Tuple<LegionDataIn, int>>();
        StreamReader dataInReader;
        StreamWriter dataOutWriter;

        Configuration _Configuration;

        public Server(string configFilename = "config.cfg")
        {
            _CurrentTaskParameter = 0;
            _CurrentTask = 0;
            _Configuration = new Configuration(configFilename);
            InitTasks();
            InitStreams();
        }

        private void InitTasks()
        {
            Field legionServerTasksField = _Configuration.GetField("legion.server.tasks");
            foreach (Field task in legionServerTasksField.Fields)
            {
                List<Field> fields = new List<Field>() { task.GetField("component") };
                LoadedComponent<LegionTask> component =
                    Loader.GetComponentsFromConfiguration<LegionTask>(fields).First();
                _Tasks.Add(component);

                List<string> paramIn = new List<string>();
                foreach (Field param in task.GetField("data_in").Fields)
                {
                    paramIn.Add(param.Value);
                }
                _TasksInputParameters.Add(paramIn);
                _TasksOutputParameters.Add(task.GetField("data_out").Value);
            }
        }

        private void InitStreams()
        {
            if (_Tasks.Count > 0 && _TasksInputParameters.Count > 0
                && _TasksInputParameters[0].Count > 0 &&
                _TasksOutputParameters.Count > 0)
            {
                dataInReader = new StreamReader(_TasksInputParameters[0][0]);
                dataOutWriter = new StreamWriter(_TasksOutputParameters[0]);

            }
        }

        internal LegionDataIn CurrentDataIn
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)CurrentTaskSingleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataIn)Activator.CreateInstance(attr.TypeIn);
            }
        }
        internal LegionDataOut CurrentDataOut
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)CurrentTaskSingleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataOut)Activator.CreateInstance(attr.TypeOut);
            }
    }

        internal void RaiseError(Exception exc)
        {
            LoggingManager.LogCritical(exc.Message);
        }

        internal LegionTask CurrentTaskSingleton
        {
            get
            {
                return CurrentTask.Singleton;
            }
        }

        internal LoadedComponent<LegionTask> CurrentTask
        {
            get
            {
                return _Tasks[_CurrentTask];
            }
        }

        internal void CheckNextInputParameters()
        {
            if (dataInReader.EndOfStream &&
                _CurrentTaskParameter + 1< _TasksInputParameters[_CurrentTask].Count)
            {
                _CurrentTaskParameter++;
                dataInReader = new StreamReader(
                   _TasksInputParameters[_CurrentTask][_CurrentTaskParameter]);
            }
        }

        internal List<LegionDataIn> GetDataIn(int taskCount)
        {
            
            lock (dataInReader)
            {
                List<LegionDataIn> result = new List<LegionDataIn>(taskCount);
                CheckNextInputParameters();
                for (int i = 0; i < taskCount && !dataInReader.EndOfStream; i++)
                {
                    LegionDataIn dataIn = CurrentDataIn;
                    dataIn.LoadFromStream(dataInReader);
                    result.Add(dataIn);

                }

                return result;
            }
        }

        internal void SaveResults(List<LegionDataOut> dataOut)
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
