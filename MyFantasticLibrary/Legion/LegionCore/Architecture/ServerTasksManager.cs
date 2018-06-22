using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
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

        public Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                return Tuple.Create(_CurrentTaskId, _Tasks[_CurrentTaskId].Task);
            }
        }

        internal ServerTasksManager(string configFilename = "config.cfg")
        {
            _CurrentTaskId = 0;
            _Tasks = new List<ServerTask>();
            _Configuration = new Configuration(configFilename);
            InitTasks();
            
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

                _Tasks.Add(new ServerTask(component, paramsIn, task.GetField("data_out").Value));
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
