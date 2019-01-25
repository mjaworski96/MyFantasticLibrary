using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComponentsLoader;
using LegionContract;
using LegionCore.Logging;

namespace LegionCore.Architecture.Server
{
    /// <summary>
    /// Legion server manager
    /// </summary>
    public class LegionServer : IDisposable
    {
        /// <summary>
        /// True if all tasks are finised, false otherwise
        /// </summary>
        public bool Finished { get; private set; }

        private ServerTasksManager _ServerTasksManager;
        private LoggingManager _LoggingManager;
        private Semaphore _EndSemaphore;
        
        /// <summary>
        /// Currently active task
        /// </summary>
        public Tuple<int, LoadedComponent<LegionTask>> CurrentTask
        {
            get
            {
                return _ServerTasksManager.CurrentTask;
            }
        }

        internal LegionServer(Semaphore endSemaphore, string configFilename = "config.xml")
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
                LoggingManager.Instance.LogInformation("[ Server ] Legion client started working.");
                semaphore.WaitOne();
                LoggingManager.Instance.LogWarning("[ Server ] Legion server finished working.");
            }           
        }

        internal void RaiseInitializationError(Tuple<int, Exception> error)
        {
            _ServerTasksManager.OnInitializationError(error.Item1);
            _LoggingManager.LogCritical($"[ Server ] Task (id = {error.Item1}) initialization error, " +
                $"exception: {error.Item2.Message}\n caused by: {error.Item2?.InnerException.Message}" +
                $"\n{error.Item2?.InnerException.StackTrace}");
        }
        /// <summary>
        /// Starts new server
        /// </summary>
        /// <param name="configFilename">Path to file with configuration</param>
        /// <returns>Running task that hosts server and server</returns>
        public static Tuple<Task, LegionServer> StartNew(string configFilename = "config.xml")
        {
            Semaphore semaphore = new Semaphore(0, Int32.MaxValue);
            LegionServer server = new LegionServer(semaphore, configFilename);
            return Tuple.Create(Task.Run(() => Work(semaphore, server)), server);
        }
        internal void RaiseError(Tuple<int, int, Exception> error)
        {
            _LoggingManager.LogError($"[ Server ] Task execution error (taskId: {error.Item1}, parameterId: {error.Item2}): " +
                $"{error.Item3.Message} \n {error.Item3.StackTrace}");
        }

        internal List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            return _ServerTasksManager.GetDataIn(tasks);
        }

        internal void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            _ServerTasksManager.SaveResults(dataOut);
        }
        /// <summary>
        /// Disposes server
        /// </summary>
        public void Dispose()
        {
            _ServerTasksManager.Dispose();
        }
    }
}
