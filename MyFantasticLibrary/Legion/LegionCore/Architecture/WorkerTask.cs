using LegionContract;
using System;
using System.Threading.Tasks;

namespace LegionCore.Architecture
{
    internal class WorkerTask
    {
        private LegionTask _myTask;
        private Task<LegionDataOut> _myRunningTask;

        public bool IsCompleted { get => _myRunningTask.IsCompleted; }

        internal WorkerTask(LegionTask myTask)
        {
            _myTask = myTask;
        }

        internal Task<LegionDataOut> Run(LegionDataIn dataIn)
        {
            _myRunningTask = Task.Run(() => _myTask.Run(dataIn));
            return _myRunningTask;
        }

        public int Id { get => IdManagement.GetId(_myTask); set => IdManagement.SetId(_myTask, value); }
        public LegionDataOut Result { get => _myRunningTask.Result; }
        public Task<LegionDataOut> MyTask { get => _myRunningTask; }
    }
}
