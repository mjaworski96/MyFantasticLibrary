using ComponentsLoader;
using LegionContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegionCore.Architecture
{
    public class Client
    {
        private WorkerTask[] _tasks;
        private IClientCommunicator _communicator;
        public int TaskCount { get => _tasks.Length;  }

        public Client(IClientCommunicator communicator, int tasksCount)
        {
            _communicator = communicator;
            _tasks = new WorkerTask[tasksCount];
        }
        public void Init()
        {
            try
            {
                LoadedComponent<LegionTask> loadedComponent
                = _communicator.CurrentTask;
                for (int i = 0; i < _tasks.Length; i++)
                {
                    _tasks[i] = new WorkerTask(loadedComponent.NewInstantion);
                    _tasks[i].Id = i;
                }
                LoggingManager.LogInformation("Legion Client initialization completed.");
            }
            catch(NullReferenceException e)
            {
                Exception exc = new LegionException("Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.", e);
                _communicator.RaiseError(exc);
                LoggingManager.LogCritical("Can not initialize Legion Client. Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.");
                throw exc;
            }
            
        }
        public void Run()
        {
            List<LegionDataIn> dataIn = new List<LegionDataIn>();
            List<LegionDataOut> dataOut = new List<LegionDataOut>();
            dataIn = _communicator.GetDataIn(TaskCount);
            Task<LegionDataOut>[] runningTasks = new Task<LegionDataOut>[dataIn.Count];
            for (int i = 0; i < dataIn.Count; i++)
            {
                runningTasks[i] =
                    _tasks[i].Run(dataIn[i]);
            }

            bool finished = false;
            LoggingManager.LogInformation("Legion Client initialized.");
            while(!finished)
            {
                Task.WaitAny(runningTasks);
                
                List<int> finishedTasks = _tasks
                    .Where(task => task.IsCompleted)
                    .Select(task => task.Id)
                    .ToList();

                LoggingManager.LogInformation("Tasks finished: " + finishedTasks.Count);

                dataOut = _tasks
                    .Where(task => finishedTasks.Contains(task.Id))
                    .Select(task => task.Result)
                    .ToList();
                _communicator.SaveResults(dataOut);
                dataIn = _communicator.GetDataIn(finishedTasks.Count);

                if (dataIn.Count != finishedTasks.Count)
                    finished = true;

                for (int i = 0; i < dataIn.Count; i++)
                {
                    runningTasks[finishedTasks[i]] = 
                        _tasks[finishedTasks[i]].Run(dataIn[i]);
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
                        notInitializedTasks.Add(_tasks[finishedTasks[i]].Id);
                    }
                    runningTasks = _tasks
                        .Where(task => notInitializedTasks.Contains(task.Id) == false)
                        .Select(task => task.MyTask)
                        .ToArray();
                }
            }
            Task.WaitAll(runningTasks);
            dataOut = runningTasks
                .Select(task => task.Result)
                .ToList();
            _communicator.SaveResults(dataOut);
            LoggingManager.LogInformation("Legion Client ends working.");
        }
    }
}
