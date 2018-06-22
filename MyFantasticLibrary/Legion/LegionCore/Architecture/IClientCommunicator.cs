using System;
using System.Collections.Generic;
using ComponentsLoader;
using LegionContract;

namespace LegionCore.Architecture
{
    public interface IClientCommunicator
    {
        LoadedComponent<LegionTask> CurrentTask { get; }

        void RaiseError(Exception exc);
        List<LegionDataIn> GetDataIn(int taskCount);
        void SaveResults(List<Tuple<int, LegionDataOut>> dataOut);
    }
}
