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
        private InMemoryServerManager _ServerManager;

        public InMemoryClientCommunicator(InMemoryServerManager serverManager)
        {
            _ServerManager = serverManager;
        }

        public Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                return _ServerManager.CurrentTask;
            }
            
        }

        public List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            return _ServerManager.GetDataIn(tasks);
        }

        public void RaiseError(Exception exc)
        {
            _ServerManager.RaiseError(exc);
        }

        public void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            _ServerManager.SaveResults(dataOut);
        }
    }
}
