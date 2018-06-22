using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using LegionCore.Logging;

namespace LegionCore.Architecture
{
    public class Server : IDisposable
    {
        private ServerTasksManager _ServerTasksManager;
        private LoggingManager _LoggingManager;

        public Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                return _ServerTasksManager.CurrentTask;
            }
        }

        public Server(string configFilename = "config.cfg")
        {
            _LoggingManager = LoggingManager.Instance;
            _ServerTasksManager = new ServerTasksManager(configFilename);
        }

        internal void RaiseError(Exception exc)
        {
            _LoggingManager.LogCritical(exc.Message);
        }

        internal List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            return _ServerTasksManager.GetDataIn(tasks);
        }

        internal void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            _ServerTasksManager.SaveResults(dataOut);
        }

        public void Dispose()
        {
            _ServerTasksManager.Dispose();
        }
    }
}
