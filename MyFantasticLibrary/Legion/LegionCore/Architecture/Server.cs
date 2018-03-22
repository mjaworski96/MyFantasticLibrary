using System;
using ComponentsLoader;
using LegionContract;

namespace LegionCore.Architecture
{
    public class Server
    {
        internal LoadedComponent<LegionTask> CurrentTask
        {
            get
            {
                return Loader.GetComponentByName<LegionTask>("Add And Wait Task");
            }
        }
    }
}
