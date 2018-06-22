using LegionContract;
using System;
using System.Threading.Tasks;

namespace LegionCore.Architecture
{
    internal class WorkerTask
    {
        private LegionTask _MyTask;
        private Task<LegionDataOut> _MyRunningTask;
        public bool Enabled { get; set; }

        public bool IsCompleted { get => _MyRunningTask.IsCompleted; }

        internal WorkerTask(LegionTask myTask)
        {
            _MyTask = myTask;
        }

        internal Task<LegionDataOut> Run(LegionDataIn dataIn)
        {
            Enabled = true;
            _MyRunningTask = Task.Run(() => _MyTask.Run(dataIn));
            return _MyRunningTask;
        }

        public int ServerSideId { get => IdManagement.GetId(_MyTask); set => IdManagement.SetId(_MyTask, value); }
        public LegionDataOut Result { get => _MyRunningTask.Result; }
        public Task<LegionDataOut> MyRunningTask { get => _MyRunningTask; }
        public int ClientSideId { get; set; }
    }
}
