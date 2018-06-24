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
        public int TaskCount { get => _Tasks.Length; }

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
        private void Init(out WorkerTask workerTask, Tuple<int,
            LoadedComponent<LegionTask>> loadedComponent,
            int id)
        {
            try
            {
                workerTask = new WorkerTask(loadedComponent.Item2.NewInstantion);
                workerTask.ServerSideId = loadedComponent.Item1;
                workerTask.ClientSideId = id;
            }
            catch (NullReferenceException e)
            {
                Exception exc = new LegionException("Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.", e);
                _Communicator.RaiseError(exc);
                _LoggingManager.LogCritical("[ Client ] Can not initialize Legion Client. Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut.");
                throw exc;
            }
        }
        public void Init()
        {

            Tuple<int, LoadedComponent<LegionTask>> loadedComponent
            = _Communicator.CurrentTask;
            for (int i = 0; i < _Tasks.Length; i++)
            {
                Init(out _Tasks[i], loadedComponent, i);
            }
            _LoggingManager.LogInformation("[ Client ] Legion Client initialization completed.");

        }

        public void Run()
        {
            if (!InitTasks())
            {
                _LoggingManager.LogWarning("[ Client ] No params available.");
                return;
            }
            bool finished = false;

            while (!finished)
            {
                Wait();
                List<Tuple<int, int>> finishedTasksIds = FinishedTasksIds;
                SendOutputDataToServer(finishedTasksIds.Select(x => x.Item2).ToList());
                ReinitializeTasksParameters(finishedTasksIds);
                finished = CheckIfFinish();
            }
            _LoggingManager.LogInformation("[ Client ] Legion Client finished working.");
        }
        private List<int> ListOfRequiredTasksParameters(int taskId)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < TaskCount; i++)
            {
                result.Add(taskId);
            }
            return result;

        }
        private bool InitTasks()
        {
            int? taskId = _Tasks.FirstOrDefault()?.ServerSideId;
            if (taskId == null) return false;

            List<LegionDataIn> dataIn = _Communicator.GetDataIn(
                ListOfRequiredTasksParameters(taskId.Value));

            if (dataIn.Count == 0)
                return false;

            for (int i = 0; i < dataIn.Count; i++)
            {
                if (dataIn[i] != null)
                    _Tasks[i].Run(dataIn[i]);
            }
            _LoggingManager.LogInformation("[ Client ] Legion Client initialized.");
            return true;
        }

        private bool CheckIfFinish()
        {
            if (_Tasks.Count(x => x.IsCompleted && !x.Enabled) == _Tasks.Count())
                return true;
            return false;
        }

        private void ReinitializeTasksParameters(List<Tuple<int, int>> finishedTasksIds)
        {
            List<LegionDataIn> dataIn =
                _Communicator.GetDataIn(finishedTasksIds
                .Select(x => x.Item1).ToList());

            bool availableNewTasks = true;
            for (int i = 0; i < dataIn.Count; i++)
            {
                if (dataIn[i] != null)
                    _Tasks[finishedTasksIds[i].Item2].Run(dataIn[i]);
                else if (availableNewTasks)
                {
                    availableNewTasks =
                       ReInit(ref _Tasks[finishedTasksIds[i].Item2]);
                }
            }
            if (availableNewTasks)
                ReinitializeDisabledTasks();
            _LoggingManager.LogInformation("[ Client ] Tasks reinitialized.");
        }

        private void ReinitializeDisabledTasks()
        {
            List<int> tasksIds = _Tasks.Where(x => !x.Enabled)
                .Select(x => x.ClientSideId).ToList();

            foreach (var task in tasksIds)
            {
                if (!ReInit(ref _Tasks[task]))
                    break;
            }
        }

        private bool ReInit(ref WorkerTask workerTask)
        {
            Tuple<int, LoadedComponent<LegionTask>> loadedComponent
           = _Communicator.CurrentTask;
            if (loadedComponent != null && loadedComponent.Item1 != workerTask.ServerSideId)
            {
                try
                {
                    Init(out workerTask, loadedComponent, workerTask.ClientSideId);
                    LegionDataIn dataIn =
                    _Communicator.GetDataIn(new List<int>() { workerTask.ServerSideId })
                    .FirstOrDefault();
                    if (dataIn != null)
                    {
                        workerTask.Run(dataIn);
                        _LoggingManager.LogInformation("[ Client ] Started new task.");
                        return true;
                    }
                    _LoggingManager.LogInformation("[ Client ] New task not available.");
                    return false;
                }
                catch (LegionException)
                {
                    return false;
                }
            }
            _LoggingManager.LogInformation("[ Client ] New task not available.");
            return false;
        }

        private void Wait()
        {
            Task.WaitAny(_Tasks
                    .Where(task => task.Enabled)
                    .Select(task => task.MyRunningTask)
                    .ToArray());
        }
        /// <summary>
        /// Contains ServerSideId and ClientSideId
        /// </summary>
        private List<Tuple<int, int>> FinishedTasksIds
        {
            get
            {
                List<Tuple<int, int>> ids = _Tasks
                   .Where(task => task.IsCompleted && task.Enabled)
                   .Select(task => Tuple.Create(task.ServerSideId, task.ClientSideId))
                   .ToList();
                _LoggingManager.LogInformation("[ Client ] Tasks finished: " + ids.Count);
                foreach (var task in ids)
                {
                    _Tasks.Where(t => t.ClientSideId == task.Item2).FirstOrDefault().Enabled = false;
                }
                return ids;
            }
        }
        private void SendOutputDataToServer(List<int> finishedTasksIds)
        {
            List<Tuple<int, LegionDataOut>> dataOut = _Tasks
                    .Where(task => finishedTasksIds.Contains(task.ClientSideId))
                    .Select(task => Tuple.Create(task.ServerSideId, task.Result))
                    .ToList();
            for (int i = 0; i < dataOut.Count; i++)
            {
                IdManagement.SetId(dataOut[i].Item2,
                    _Tasks[finishedTasksIds[i]].ParameterId);
            }
            

            _Communicator.SaveResults(dataOut);
        }
    }
}
