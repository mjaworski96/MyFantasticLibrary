using ComponentsLoader;
using LegionContract;
using LegionCore.Architecture.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegionCore.InMemoryCommunication
{
    public class InMemoryServerManager : IServerCommunicator
    {
        private LegionServer _Server;

        public InMemoryServerManager(LegionServer server)
        {
            _Server = server;
        }

        public Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask()
        {
            return _Server.CurrentTask;
        }

        public List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            return _Server.GetDataIn(tasks);
        }

        public void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            _Server.SaveResults(dataOut);
        }

        public void RaiseError((int TaskId, int ParameterId, Exception exception) error)
        {
            _Server.RaiseError(error);
        }

        public void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            _Server.RaiseInitializationError(exceptionTaskId);
        }
    }
}
