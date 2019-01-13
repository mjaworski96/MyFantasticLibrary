using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using LegionCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LegionCore.Architecture.Server
{
    internal class ServerTasksManager : IDisposable
    {
        private int _CurrentTaskId;
        private List<ServerTask> _Tasks;
        private Configuration _Configuration;
        private LoggingManager _Logger;
        private LegionServer _Server;
        private object _CurrentTaskIdLock;
        public Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                lock (_CurrentTaskIdLock)
                {
                    return CurrentTimeoutedTask ?? CurrentNormalTask;
                }
            }
        }

        private Tuple<int, LoadedComponent<LegionTask>> CurrentNormalTask
        {
            get
            {
                if (_Tasks[_CurrentTaskId].NoParametersAvailable && !_Tasks[_CurrentTaskId].HasAnyTimeouts)
                {
                    if (_CurrentTaskId < _Tasks.Count - 1)
                    {
                        _Logger.LogInformation("[ Server ] Started new task.");
                        _CurrentTaskId++;
                    }
                    else
                    {
                        _Logger.LogInformation("[ Server ] No more tasks.");
                        return null;
                    }
                }

                return Tuple.Create(_CurrentTaskId, _Tasks[_CurrentTaskId].Task);

            }
        }
        private Tuple<int, LoadedComponent<LegionTask>> CurrentTimeoutedTask
        {
            get
            {
                for (int i = 0; i < _CurrentTaskId; i++)
                {
                    if (_Tasks[i].HasAnyTimeouts)
                        return Tuple.Create(i, _Tasks[i].Task);
                }
                return null;
            }
        }
        internal ServerTasksManager(LegionServer server, string configFilename = "config.xml")
        {
            _CurrentTaskIdLock = new object();
            _Server = server;
            _Logger = LoggingManager.Instance;
            _CurrentTaskId = 0;
            _Tasks = new List<ServerTask>();
            _Configuration = new Configuration(configFilename);
            InitTasks();

        }

        internal void OnInitializationError(int invalidTaskId)
        {
            lock (_CurrentTaskIdLock)
            {
                if (_CurrentTaskId == invalidTaskId)
                {
                    _Tasks[_CurrentTaskId].Error = true;
                    if (_CurrentTaskId < _Tasks.Count - 1)
                        _CurrentTaskId++;
                    CheckIfFinish();
                }
            }
        }

        internal void CheckIfFinish()
        {
            foreach (var task in _Tasks)
            {
                if (!task.CheckIfFinish())
                    return;
            }
            _Server.Finish();
        }
        private void InitTasks()
        {
            Field legionServerTasksField = _Configuration.GetField("legion.server.tasks");
            foreach (Field task in legionServerTasksField.Fields)
            {
                List<Field> fields = new List<Field>() { task.GetField("component") };
                LoadedComponent<LegionTask> component =
                    Loader.GetComponentsFromConfiguration<LegionTask>(fields).First();

                List<string> paramsIn = task
                    .GetField("data_in")
                    .Fields
                    .Select(x => x.Value)
                    .ToList();

                _Tasks.Add(new ServerTask(component, paramsIn, task.GetField("data_out").Value,
                    task.GetField("ordered_data_out").Value, long.Parse(task.GetField("timeout").Value ?? "0")));
            }
        }
        internal static List<LegionDataIn> GetEmptyDataIn(List<int> tasks)
        {
            List<LegionDataIn> result = new List<LegionDataIn>(tasks.Count);
            foreach (var item in tasks)
            {
                result.Add(null);
            }
            return result;
        }
        internal List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            List<LegionDataIn> result = new List<LegionDataIn>(tasks.Count);
            foreach (var item in tasks)
            {
                if (_Tasks.Count > item)
                {
                    LegionDataIn dataIn = _Tasks[item].GetDataIn();
                    if (dataIn is LegionErrorDataIn errorDataIn)
                        HandleTaskParameterError(item, IdManagement.GetId(errorDataIn), errorDataIn.TransformToDataOut());
                    else
                        result.Add(dataIn);
                }
                else
                {
                    result.Add(null);
                }
            }

            return result;
        }
        internal void HandleTaskParameterError(int taskId, int parameterId, LegionErrorDataOut errorData)
        {
            IdManagement.SetId(errorData, parameterId);
            SaveResults(new List<Tuple<int, LegionDataOut>>()
            {
                Tuple.Create(taskId, errorData as LegionDataOut)
            });
        }
        internal void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            IEnumerable<int> finishedTasks = dataOut.Select(x => x.Item1).Distinct();
            foreach (var item in finishedTasks)
            {
                if (_Tasks.Count > item)
                    _Tasks[item].SaveResults(dataOut
                        .Where(x => x.Item1 == item)
                        .Select(x => x.Item2));
            }
            CheckIfFinish();
        }

        public void Dispose()
        {
            foreach (var item in _Tasks)
            {
                item.Dispose();
            }
        }
    }
}
