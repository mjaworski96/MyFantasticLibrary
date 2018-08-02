using ComponentsLoader;
using LegionContract;
using LegionCore.Architecture.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegionCore.InMemoryCommunication
{
    public class InMemoryServerManager
    {
        private LegionServer _Server;

        public InMemoryServerManager(LegionServer server)
        {
            _Server = server;
        }

        internal Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                return _Server.CurrentTask;
            }
            
        }

        internal List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            return _Server.GetDataIn(tasks);
        }

        internal void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            _Server.SaveResults(dataOut);
        }

        internal void RaiseError(Exception exc)
        {
            _Server.RaiseError(exc);
        }

        internal void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            _Server.RaiseInitializationError(exceptionTaskId);
        }
    }
}
