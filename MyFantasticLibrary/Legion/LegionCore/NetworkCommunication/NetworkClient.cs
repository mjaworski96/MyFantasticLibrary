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
    public class NetworkClient : IClientCommunicator
    {
        private Sender _Sender;
        private Dictionary<string, Tuple<int, LoadedComponent<LegionTask>>> _KnownComponents;

        public NetworkClient(string configFilename = "config.xml")
        {
            _KnownComponents = new Dictionary<string, Tuple<int, LoadedComponent<LegionTask>>>();
            Configuration configuration = new Configuration(configFilename);
            _Sender = new Sender(
                configuration.GetString("legion.network.address"),
                int.Parse(configuration.GetString("legion.network.port")));
        }

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
