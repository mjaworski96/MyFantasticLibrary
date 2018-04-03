using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegionCore.Architecture
{
    public class Client
    {
        private WorkerTask[] _Tasks;
        private IClientCommunicator _Communicator;
        public int TaskCount { get => _Tasks.Length;  }

        private Client(IClientCommunicator communicator)
        {
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
                LoggingManager.LogInformation("Legion Client initialization completed.");
            }
            catch(NullReferenceException e)
            {
                Exception exc = new LegionException("Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.", e);
                _Communicator.RaiseError(exc);
                LoggingManager.LogCritical("Can not initialize Legion Client. Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.");
                throw exc;
            }

        }
        public void Run()
        {
            List<LegionDataIn> dataIn = new List<LegionDataIn>();
            List<LegionDataOut> dataOut = new List<LegionDataOut>();
            dataIn = _Communicator.GetDataIn(TaskCount);
            Task<LegionDataOut>[] runningTasks = new Task<LegionDataOut>[dataIn.Count];
            for (int i = 0; i < dataIn.Count; i++)
            {
                runningTasks[i] =
                    _Tasks[i].Run(dataIn[i]);
            }

            bool finished = false;
            LoggingManager.LogInformation("Legion Client initialized.");
            while(!finished)
            {
                Task.WaitAny(runningTasks);
                
                List<int> finishedTasks = _Tasks
                    .Where(task => task.IsCompleted)
                    .Select(task => task.Id)
                    .ToList();

                LoggingManager.LogInformation("Tasks finished: " + finishedTasks.Count);

                dataOut = _Tasks
                    .Where(task => finishedTasks.Contains(task.Id))
                    .Select(task => task.Result)
                    .ToList();
                _Communicator.SaveResults(dataOut);
                dataIn = _Communicator.GetDataIn(finishedTasks.Count);

                if (dataIn.Count != finishedTasks.Count)
                    finished = true;

                for (int i = 0; i < dataIn.Count; i++)
                {
                    runningTasks[finishedTasks[i]] = 
                        _Tasks[finishedTasks[i]].Run(dataIn[i]);
                }

                LoggingManager.LogInformation("Tasks reinitialized.");

                if (finished)
                {
                    LoggingManager.LogInformation("No more parameters left.");
                    int firstNotInitializedTask =
                        finishedTasks.Count - dataIn.Count;
                    List<int> notInitializedTasks = new List<int>();
                    for (int i = firstNotInitializedTask; i < finishedTasks.Count; i++)
                    {
                        notInitializedTasks.Add(_Tasks[finishedTasks[i]].Id);
                    }
                    runningTasks = _Tasks
                        .Where(task => notInitializedTasks.Contains(task.Id) == false)
                        .Select(task => task.MyTask)
                        .ToArray();
                }
            }
            Task.WaitAll(runningTasks);
            dataOut = runningTasks
                .Select(task => task.Result)
                .ToList();
            _Communicator.SaveResults(dataOut);
            LoggingManager.LogInformation("Legion Client ends working.");
        }
    }
}
