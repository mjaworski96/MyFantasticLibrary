using System;
using System.Collections.Generic;
using ComponentsLoader;
using LegionContract;

namespace LegionCore.Architecture.Client
{
    public interface IClientCommunicator
    {
        Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask();
        void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId);
        void RaiseError((int TaskId, int ParameterId, Exception exception) error);
        List<LegionDataIn> GetDataIn(List<int> tasks);
        void SaveResults(List<Tuple<int, LegionDataOut>> dataOut);
    }
}
