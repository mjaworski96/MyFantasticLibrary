using System;
using System.Collections.Generic;
using ComponentsLoader;
using LegionContract;

namespace LegionCore.Architecture.Client
{
    /// <summary>
    /// Intrface for client-server communication
    /// </summary>
    public interface IClientCommunicator
    {
        /// <summary>
        /// Gets current task
        /// </summary>
        /// <returns>Id of currently runned task (server side) and <see cref="LoadedComponent{T}"/> with task</returns>
        Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask();
        /// <summary>
        /// Send error that occurs when client is initialized
        /// </summary>
        /// <param name="error">Id (server side) and exception that causes error</param>
        void RaiseInitializationError(Tuple<int, Exception> error);
        /// <summary>
        /// Send error that occurs when running task
        /// </summary>
        /// <param name="error">TaskId (server side), parameter id, and exception that causes error</param>
        void RaiseError(Tuple<int, int, Exception> error);
        /// <summary>
        /// Gets data in for tasks
        /// </summary>
        /// <param name="tasks">Id of tasks(server side)</param>
        /// <returns>Input data for tasks</returns>
        List<LegionDataIn> GetDataIn(List<int> tasks);
        /// <summary>
        /// Sends results
        /// </summary>
        /// <param name="dataOut">Id of task (server side) and result</param>
        void SaveResults(List<Tuple<int, LegionDataOut>> dataOut);
    }
}
