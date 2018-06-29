using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using LegionCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LegionCore.Architecture
{
    internal class ServerTasksManager: IDisposable
    {
        private int _CurrentTaskId;
        private List<ServerTask> _Tasks;
        private Configuration _Configuration;
        private LoggingManager _Logger;
        private Server _Server;

        public Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                if (_Tasks[_CurrentTaskId].NoParametersAvailable)
                {
                    if(_CurrentTaskId < _Tasks.Count - 1)
                    {
                        _Logger.LogInformation("[ Server ] Started new task.");
                        _CurrentTaskId++;
                    }
                    else
                    {
                        _Logger.LogInformation("[ Server ] No more tasks.");
                    }
                }
                   
                return Tuple.Create(_CurrentTaskId, _Tasks[_CurrentTaskId].Task);
            }
        }

        internal ServerTasksManager(Server server, string configFilename = "config.cfg")
        {
            _Server = server;
            _Logger = LoggingManager.Instance;
            _CurrentTaskId = 0;
            _Tasks = new List<ServerTask>();
            _Configuration = new Configuration(configFilename);
            InitTasks();
            
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
                    task.GetField("ordered_data_out").Value));
            }
        }
        internal List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            List<LegionDataIn> result = new List<LegionDataIn>(tasks.Count);
            foreach (var item in tasks)
            {
                if (_Tasks.Count > item)
                {
                    result.Add(_Tasks[item].GetDataIn());
                }
                else
                {
                    result.Add(null);
                }
            }
            
            return result;
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
