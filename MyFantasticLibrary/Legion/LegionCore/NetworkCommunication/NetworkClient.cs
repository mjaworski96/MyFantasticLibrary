using System;
using System.Collections.Generic;
using ComponentsLoader;
using LegionContract;
using LegionCore.Architecture.Client;

namespace LegionCore.NetworkCommunication
{
    public class NetworkClient : IClientCommunicator
    {
        public Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask()
        {
            throw new NotImplementedException();
        }

        public List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            throw new NotImplementedException();
        }

        public void RaiseError(Exception exc)
        {
            throw new NotImplementedException();
        }

        public void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            throw new NotImplementedException();
        }

        public void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            throw new NotImplementedException();
        }
    }
}
