using ComponentsLoader;
using LegionContract;
using LegionCore.Architecture.Client;
using LegionCore.Architecture.Server;
using System;
using System.Collections.Generic;

namespace LegionCore.InMemoryCommunication
{
    /// <summary>
    /// In memory client-server communicator
    /// </summary>
    public class InMemoryClientCommunicator : IClientCommunicator
    {
        private LegionServer _Server;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server to communicate</param>
        public InMemoryClientCommunicator(LegionServer server)
        {
            _Server = server;
        }
        /// <summary>
        /// Gets current task
        /// </summary>
        /// <returns>Id of currently runned task (server side) and <see cref="LoadedComponent{T}"/> with task</returns>
        public Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask()
        {
            return _Server.CurrentTask;
        }
        /// <summary>
        /// Gets data in for tasks
        /// </summary>
        /// <param name="tasks">Id of tasks(server side)</param>
        /// <returns>Input data for tasks</returns>
        public List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            return _Server.GetDataIn(tasks);
        }
        /// <summary>
        /// Send error that occurs when running task
        /// </summary>
        /// <param name="error">TaskId (server side), parameter id, and exception</param>
        public void RaiseError((int TaskId, int ParameterId, Exception exception) error)
        {
            _Server.RaiseError(error);
        }
        /// <summary>
        /// Send error that occurs when client is initialized
        /// </summary>
        /// <param name="exceptionTaskId">Exception and taks id (server side)</param>
        public void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            _Server.RaiseInitializationError(exceptionTaskId);
        }
        /// <summary>
        /// Sends results
        /// </summary>
        /// <param name="dataOut">Id of task (server side) and result</param>
        public void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            _Server.SaveResults(dataOut);
        }
    }
}
