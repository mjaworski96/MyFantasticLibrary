using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using LegionCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegionCore.Architecture
{
    public class Client
    {
        private LoggingManager _LoggingManager;
        private WorkerTask[] _Tasks;
        private IClientCommunicator _Communicator;
        public int TaskCount { get => _Tasks.Length;  }

        private Client(IClientCommunicator communicator)
        {
            _LoggingManager = LoggingManager.Instance;
            _Communicator = communicator;
        }

        public Client(IClientCommunicator communicator, int tasksCount)
            : this(communicator)
        {
            _Tasks = new WorkerTask[tasksCount];
        }
        public Client(IClientCommunicator communicatoror, string configFilename = "config.cfg")
            : this(communicatoror)
        {
            Configuration configuration = new Configuration(configFilename);
            int tasksCount = int.Parse(configuration.GetString("legion.client.workers"));
            _Tasks = new WorkerTask[tasksCount];
        }
        public void Init()
        {
            try
            {
                LoadedComponent<LegionTask> loadedComponent
                = _Communicator.CurrentTask;
                for (int i = 0; i < _Tasks.Length; i++)
                {
                    _Tasks[i] = new WorkerTask(loadedComponent.NewInstantion);
                    _Tasks[i].Id = i;
                }
                _LoggingManager.LogInformation("Legion Client initialization completed.");
            }
            catch(NullReferenceException e)
            {
                Exception exc = new LegionException("Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.", e);
                _Communicator.RaiseError(exc);
                _LoggingManager.LogCritical("Can not initialize Legion Client. Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.");
                throw exc;
            }

        }

        public void Run()
        {
            if(!InitTasks())
            {
                _LoggingManager.LogWarning("No params available.");
                return;
            }
            bool noMoreParameters = false;
            bool finished = false;

            while(!finished)
            {
                Wait();

                CheckIfFinish(noMoreParameters, ref finished);
                List<int> finishedTasksIds = FinishedTasksIds;
                SendOutputDataToServer(finishedTasksIds);
                if (!noMoreParameters)
                    ReinitializeTasks(finishedTasksIds, ref noMoreParameters);  
            }
            _LoggingManager.LogInformation("Legion Client ended working.");
        }

        private bool InitTasks()
        {
            List<LegionDataIn> dataIn = _Communicator.GetDataIn(TaskCount);
            if (dataIn.Count == 0)
                return false;
            for (int i = 0; i < dataIn.Count; i++)
            {
                _Tasks[i].Run(dataIn[i]);
            }
            _LoggingManager.LogInformation("Legion Client initialized.");
            return true;
        }

        private void CheckIfFinish(bool noMoreParameters, ref bool finished)
        {
            if (noMoreParameters && _Tasks.Count(x => x.IsCompleted) == _Tasks.Count())
                finished = true;
        }

        private void ReinitializeTasks(List<int> finishedTasksIds, ref bool noMoreParameters)
        {
            List<LegionDataIn>  dataIn = 
                _Communicator.GetDataIn(finishedTasksIds.Count);

            if (dataIn.Count != finishedTasksIds.Count)
            {
                noMoreParameters = true;
                _LoggingManager.LogInformation("No more parameters left.");
            }


            for (int i = 0; i < dataIn.Count; i++)
                _Tasks[finishedTasksIds[i]].Run(dataIn[i]);

            _LoggingManager.LogInformation("Tasks reinitialized.");
        }

        private void Wait()
        {
            Task.WaitAny(_Tasks
                    .Where(task => task.Enabled)
                    .Select(task => task.MyTask)
                    .ToArray());
        }
        private List<int> FinishedTasksIds
        {
            get
            {
                List<int> ids = _Tasks
                   .Where(task => task.IsCompleted && task.Enabled)
                   .Select(task => task.Id)
                   .ToList();
                _LoggingManager.LogInformation("Tasks finished: " + ids.Count);
                foreach (var task in ids)
                {
                    _Tasks.Where(t => t.Id == task).FirstOrDefault().Enabled = false;
                }
                return ids;
            }
        }
        private void SendOutputDataToServer(List<int> finishedTasksIds)
        {
            List<LegionDataOut> dataOut = _Tasks
                    .Where(task => finishedTasksIds.Contains(task.Id))
                    .Select(task => task.Result)
                    .ToList();
            _Communicator.SaveResults(dataOut);
        }
    }
}
