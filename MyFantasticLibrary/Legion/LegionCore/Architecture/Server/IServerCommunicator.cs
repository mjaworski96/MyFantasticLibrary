using ComponentsLoader;
using LegionContract;
using System;
using System.Collections.Generic;

namespace LegionCore.Architecture.Server
{
    public interface IServerCommunicator
    {

        Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask();

        List<LegionDataIn> GetDataIn(List<int> tasks);

        void SaveResults(List<Tuple<int, LegionDataOut>> dataOut);

        void RaiseError(Exception exc);

        void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId);
    }
}
