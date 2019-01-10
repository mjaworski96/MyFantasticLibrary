using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComponentsLoader;
using LegionContract;
using LegionCore.Logging;

namespace LegionCore.Architecture.Server
{
    public class LegionServer : IDisposable
    {
        public bool Finished { get; private set; }

        private ServerTasksManager _ServerTasksManager;
        private LoggingManager _LoggingManager;
        private Semaphore _EndSemaphore;
        

        public Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                return _ServerTasksManager.CurrentTask;
            }
        }

        public LegionServer(Semaphore endSemaphore, string configFilename = "config.cfg")
        {
            Finished = false;
            _LoggingManager = LoggingManager.Instance;
            _ServerTasksManager = new ServerTasksManager(this, configFilename);
            _EndSemaphore = endSemaphore;
        }
        internal void Finish()
        {
            _EndSemaphore.Release();
            Finished = true;
        }
        private static void Work(Semaphore semaphore, LegionServer server)
        {
            using (server)
            {
                semaphore.WaitOne();
                LoggingManager.Instance.LogWarning("[ Server ] Legion server finished working.");
            }           
        }

        internal void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            _ServerTasksManager.OnInitializationError(exceptionTaskId.Item2);
            RaiseError(exceptionTaskId.Item1);
        }

        public static Tuple<Task, LegionServer> StartNew(string configFilename = "config.cfg")
        {
            Semaphore semaphore = new Semaphore(0, Int32.MaxValue);
            LegionServer server = new LegionServer(semaphore, configFilename);
            return Tuple.Create(Task.Run(() => Work(semaphore, server)), server);
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
