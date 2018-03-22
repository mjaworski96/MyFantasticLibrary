using System;
using ComponentsLoader;
using LegionContract;

namespace LegionCore.Architecture
{
    public interface IClientCommunicator
    {
        LoadedComponent<LegionTask> CurrentTask { get; }

        void RaiseError(Exception exc);
    }
}
