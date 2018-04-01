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
        private Server _Server;

        public InMemoryServerManager(Server server)
        {
            _Server = server;
        }

        internal LoadedComponent<LegionTask> CurrentTask
        {
            get
            {
                return _Server.CurrentTask;
            }
            
        }

        internal List<LegionDataIn> GetDataIn(int taskCount)
        {
            return _Server.GetDataIn(taskCount);
        }

        internal void SaveResults(List<LegionDataOut> dataOut)
        {
            _Server.SaveResults(dataOut);
        }

        internal void RaiseError(Exception exc)
        {
            _Server.RaiseError(exc);
        }
    }
}
