using LegionContract;
using LegionCore.Architecture.Error;
using System;
using System.Threading.Tasks;

namespace LegionCore.Architecture.Client
{
    internal class WorkerTask
    {
        private LegionTask _MyTask;
        private Task<LegionDataOut> _MyRunningTask;

        internal WorkerTask(LegionTask myTask)
        {
            _MyTask = myTask;
        }

        internal Task<LegionDataOut> Run(LegionDataIn dataIn)
        {
            Enabled = true;
            ParameterId = IdManagement.GetId(dataIn);
            _MyRunningTask = Task.Run(() => {
                LegionDataOut dataOut = null;
                try
                {
                    dataOut = _MyTask.Run(dataIn);
                }
                catch(Exception exc)
                {
                    dataOut = new LegionExecutionErrorDataOut(exc);
                }
                IdManagement.SetId(dataOut, ParameterId);
                return dataOut;
            });
            return _MyRunningTask;
        }
        public bool Enabled { get; set; }
        public bool IsCompleted { get => _MyRunningTask?.IsCompleted ?? true; }
        public int ServerSideId { get => IdManagement.GetId(_MyTask); set => IdManagement.SetId(_MyTask, value); }
        public LegionDataOut Result { get => _MyRunningTask.Result; }
        public Task<LegionDataOut> MyRunningTask { get => _MyRunningTask; }
        public int ClientSideId { get; set; }
        public int ParameterId { get; set; }
    }
}
