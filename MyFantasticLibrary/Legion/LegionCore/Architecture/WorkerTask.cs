using LegionContract;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LegionCore.Architecture
{
    public class WorkerTask
    {
        private LegionTask _myTask;

        public WorkerTask(LegionTask myTask)
        {
            _myTask = myTask;
        }

        public Task<LegionDataOut> Run(LegionDataIn dataIn)
        {
            return Task.Run(() => _myTask.Run(dataIn));
        }

    }
}
