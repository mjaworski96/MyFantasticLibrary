using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AplicationInformationExchange.Communication;
using AplicationInformationExchange.Model;
using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using LegionCore.Architecture.Client;

namespace LegionCore.NetworkCommunication
{
    public class NetworkClient : IClientCommunicator
    {
        private Sender _Sender;
        private Dictionary<string, Tuple<int, LoadedComponent<LegionTask>>> _KnownComponents;
        private Tuple<int, LoadedComponent<LegionTask>> GetKnownComponent(string key)
        {
            return null;
        }
        public NetworkClient(string configFilename = "config.cfg")
        {
            _KnownComponents = new Dictionary<string, Tuple<int, LoadedComponent<LegionTask>>>();
            Configuration configuration = new Configuration(configFilename);
            _Sender = new Sender(
                configuration.GetString("legion.network.address"),
                int.Parse(configuration.GetString("legion.network.port")));
        }

        public Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask()
        {
            Message response =
                _Sender.Send(Message.WithEmptyBody((int)CodeStatus.OK, 
                (int)OperationCode.GET_CURRENT_TASK));
            if(response.Header.StatusCode == (int)CodeStatus.OK)
            {
                string taskMetadata = response.Body.GetPage(0).ToObject<string>();
                if (!_KnownComponents.ContainsKey(taskMetadata))
                {
                    SaveTaskInMemory(response, taskMetadata);
                }
                return _KnownComponents[taskMetadata];
            }
            return null;
        }

        private void SaveTaskInMemory(Message response, string taskMetadata)
        {
            Message fileRequest = Message.WithEmptyBody((int)CodeStatus.OK,
                    (int)OperationCode.GET_CURRENT_TASK_FILES);
            fileRequest.AddPage(BodyPage.FromObject("id", response.Body.GetPage(1).ToObject<int>()));
            Message dlls = _Sender.Send(fileRequest);
            try { dlls.ToFiles(); } catch (IOException) { Logging.LoggingManager.Instance.LogError("Save dlls error"); }
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
            Message response = _Sender.Send(Message.FromObject("list", tasks,
               (int)CodeStatus.OK, (int)OperationCode.GET_DATA_IN));

            return response.Body.GetPage(0).ToObject<List<LegionDataIn>>();
        }

        public void RaiseError(Exception exc)
        {
            throw new NotImplementedException();
        }

        public void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            throw new NotImplementedException();
        }

        public void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            throw new NotImplementedException();
        }
    }
}
