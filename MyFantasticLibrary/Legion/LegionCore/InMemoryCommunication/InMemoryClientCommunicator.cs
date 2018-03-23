using ComponentsLoader;
using LegionContract;
using LegionCore.Architecture;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegionCore.InMemoryCommunication
{
    public class InMemoryClientCommunicator : IClientCommunicator
    {
        private InMemoryServerManager _serverManager;

        public InMemoryClientCommunicator(InMemoryServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        public LoadedComponent<LegionTask> CurrentTask
        {
            get
            {
                return _serverManager.CurrentTask;
            }
            
        }

        public List<LegionDataIn> GetDataIn(int taskCount)
        {
            return _serverManager.GetDataIn(taskCount);
        }

        public void RaiseError(Exception exc)
        {
            throw new NotImplementedException();
        }

        public void SaveResults(List<LegionDataOut> dataOut)
        {
            _serverManager.SaveResults(dataOut);
        }
    }
}
