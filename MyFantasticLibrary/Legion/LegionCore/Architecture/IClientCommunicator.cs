using System;
using System.Collections.Generic;
using ComponentsLoader;
using LegionContract;

namespace LegionCore.Architecture
{
    public interface IClientCommunicator
    {
        Tuple<int, LoadedComponent<LegionTask>> CurrentTask { get; }

        void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId);
        void RaiseError(Exception exc);
        List<LegionDataIn> GetDataIn(List<int> tasks);
        void SaveResults(List<Tuple<int, LegionDataOut>> dataOut);
    }
}
