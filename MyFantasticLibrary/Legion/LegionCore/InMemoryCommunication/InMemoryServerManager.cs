using ComponentsLoader;
using LegionContract;
using LegionCore.Architecture;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegionCore.InMemoryCommunication
{
    public class InMemoryServerManager
    {
        private Server _server;

        public InMemoryServerManager(Server server)
        {
            _server = server;
        }

        internal LoadedComponent<LegionTask> CurrentTask
        {
            get
            {
                return _server.CurrentTask;
            }
            
        }

        internal List<LegionDataIn> GetDataIn(int taskCount)
        {
            return _server.GetDataIn(taskCount);
        }

        internal void SaveResults(List<LegionDataOut> dataOut)
        {
            _server.SaveResults(dataOut);
        }
    }
}
