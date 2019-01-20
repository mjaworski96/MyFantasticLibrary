using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using AplicationInformationExchange.Communication;
using AplicationInformationExchange.Model;
using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using LegionCore.Architecture;
using LegionCore.Architecture.Client;
using LegionCore.Logging;

namespace LegionCore.NetworkCommunication
{
    /// <summary>
    /// Network client-server communicator
    /// </summary>
    public class NetworkClient : IClientCommunicator
    {
        private Sender _Sender;
        private Dictionary<string, Tuple<int, LoadedComponent<LegionTask>>> _KnownComponents;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configFilename">Path to file with configuration</param>
        public NetworkClient(string configFilename = "config.xml")
        {
            _KnownComponents = new Dictionary<string, Tuple<int, LoadedComponent<LegionTask>>>();
            Configuration configuration = new Configuration(configFilename);
            _Sender = new Sender(configFilename);
        }
        /// <summary>
        /// Get current taksk
        /// </summary>
        /// <returns><see cref="LoadedComponent{T}"/> with current task</returns>
        public Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask()
        {
            try
            {
                Message response =
               _Sender.Send(Message.WithEmptyBody((int)CodeStatus.OK,
               (int)OperationCode.GET_CURRENT_TASK));
                if (response.Header.StatusCode == (int)CodeStatus.OK)
                {
                    string taskMetadata = response.Body.GetPage(0).ToObject<string>();
                    if (!_KnownComponents.ContainsKey(taskMetadata))
                    {
                        SaveTaskInMemory(response, taskMetadata);
                    }
                    return _KnownComponents[taskMetadata];
                }
            }
            catch (SocketException)
            {
                LoggingManager.Instance.LogError("[ Client ] Server unavailable");
            }

            return null;
        }

        private void SaveTaskInMemory(Message response, string taskMetadata)
        {
            try
            {
                Message fileRequest = Message.WithEmptyBody((int)CodeStatus.OK,
                    (int)OperationCode.GET_CURRENT_TASK_FILES);
                fileRequest.AddPage(BodyPage.FromObject("id", response.Body.GetPage(1).ToObject<int>()));
                Message dlls = _Sender.Send(fileRequest);
                try
                {
                    dlls.ToFiles();
                }
                catch (IOException)
                {
                    LoggingManager.Instance.LogError("[ Client ] Save .dll error");
                }
                AddTaskToDictionary(response, taskMetadata);
            }
            catch (SocketException)
            {
                LoggingManager.Instance.LogError("[ Client ] Server unavailable");
            }
            
        }

        private void AddTaskToDictionary(Message response, string taskMetadata)
        {
            string[] splitedTaskMetadata = taskMetadata.Split(';');
            _KnownComponents.Add(taskMetadata,
                Tuple.Create(
                     response.Body.GetPage(1).ToObject<int>(),
                    Loader.GetComponentByNameVersionPublisher<LegionTask>(splitedTaskMetadata[0],
                    splitedTaskMetadata[1], splitedTaskMetadata[2])
                    ));
        }
        /// <summary>
        /// Gets input data
        /// </summary>
        /// <param name="tasks">Ids of tasks</param>
        /// <returns>Input data for tasks</returns>
        public List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            try
            {
                Message response = _Sender.Send(Message.FromObject("list", tasks,
                    (int)CodeStatus.OK, (int)OperationCode.GET_DATA_IN));
                if (response.Header.StatusCode == (int)CodeStatus.ERROR)
                    return tasks.Select(x => (LegionDataIn)null).ToList();

                return response.Body.GetPage(0).ToObject<List<LegionDataIn>>();
            }
            catch (SocketException)
            {
                LoggingManager.Instance.LogError("[ Client ] Server unavailable");
            }
            return new List<LegionDataIn>();
        }
        /// <summary>
        /// Send error
        /// </summary>
        /// <param name="error">Task id, parameter id and exception that causes error</param>
        public void RaiseError((int TaskId, int ParameterId, Exception exception) error)
        {
           try
            {
                _Sender.Send(Message.FromObject("error", error,
                    (int)CodeStatus.ERROR, (int)OperationCode.RAISE_ERROR));
            }
            catch (SocketException)
            {
                LoggingManager.Instance.LogError("[ Client ] Server unavailable");
            }
        }
        /// <summary>
        /// Send initialization error
        /// </summary>
        /// <param name="exceptionTaskId">Exception that causes error and task id</param>
        public void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            try
            {
                _Sender.Send(Message.FromObject("error", exceptionTaskId,
                    (int)CodeStatus.ERROR, (int)OperationCode.RAISE_INITIALIZATION_ERROR));
            }
            catch (SocketException)
            {
                LoggingManager.Instance.LogError("[ Client ] Server unavailable");
            }
        }
        /// <summary>
        /// Send results
        /// </summary>
        /// <param name="dataOut">Task id and result</param>
        public void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            try
            {
                _Sender.Send(Message.FromObject("results", dataOut,
                (int)CodeStatus.OK, (int)OperationCode.SAVE_RESULTS));
            }
            catch (SocketException)
            {
                LoggingManager.Instance.LogError("[ Client ] Server unavailable");
            }
        }
    }
}
