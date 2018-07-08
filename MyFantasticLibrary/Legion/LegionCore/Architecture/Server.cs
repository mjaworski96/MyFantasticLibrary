using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComponentsLoader;
using LegionContract;
using LegionCore.Logging;

namespace LegionCore.Architecture
{
    public class Server : IDisposable
    {
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

        public Server(Semaphore endSemaphore, string configFilename = "config.cfg")
        {
            _LoggingManager = LoggingManager.Instance;
            _ServerTasksManager = new ServerTasksManager(this, configFilename);
            _EndSemaphore = endSemaphore;
        }
        internal void Finish()
        {
            _EndSemaphore.Release();
        }
        private static void Work(Semaphore semaphore, Server server)
        {
            using (server)
            {
                semaphore.WaitOne();
                LoggingManager.Instance.LogInformation("[ Server ] Legion server finished working.");
            }           
        }

        internal void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            _ServerTasksManager.OnInitializationError(exceptionTaskId.Item2);
            RaiseError(exceptionTaskId.Item1);
        }

        public static Tuple<Task, Server> StartNew(string configFilename = "config.cfg")
        {
            Semaphore semaphore = new Semaphore(0, Int32.MaxValue);
            Server server = new Server(semaphore, configFilename);
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
