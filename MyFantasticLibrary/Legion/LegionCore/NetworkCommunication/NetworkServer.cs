using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AplicationInformationExchange.Communication;
using AplicationInformationExchange.Model;
using ComponentsLoader;
using ConfigurationManager;
using LegionContract;
using LegionCore.Architecture.Server;
using LegionCore.Logging;

namespace LegionCore.NetworkCommunication
{
    public class NetworkServer : IServerCommunicator
    {
        private static readonly string[] BANNED_ASSEMBLIES =
        {
            "netstandard",
            "ComponentContract",
            "LegionContract"
        };

        private Receiver _Receiver;
        private LegionServer _Server;
        private Dictionary<int, LoadedComponent<LegionTask>> _KnownComponents;
        private LoggingManager _Logger;
        public NetworkServer(LegionServer server, string configFilename = "config.xml")
        {
            _Logger = LoggingManager.Instance;
            _KnownComponents = new Dictionary<int, LoadedComponent<LegionTask>>();
            _Server = server;
            Configuration configuration = new Configuration(configFilename);
            _Receiver = new Receiver(configFilename,
                Communication,
                Finished);
        }
        public Task Start()
        {
            return Task.Run(() => _Receiver.ReceiveAll());
        }
        private bool Finished()
        {
            return _Server.Finished;
        }
        private Message Communication(Message message)
        {
            try
            {
                switch ((OperationCode)message.Header.OperationCode)
                {
                    case OperationCode.GET_CURRENT_TASK:
                        return GetCurrentTaskMetadata();
                    case OperationCode.GET_CURRENT_TASK_FILES:
                        return GetCurrentTaskFiles(message);
                    case OperationCode.GET_DATA_IN:
                        return GetDataIn(message);
                    case OperationCode.SAVE_RESULTS:
                        return SaveResults(message);
                    case OperationCode.RAISE_ERROR:
                        return RaiseError(message);
                    case OperationCode.RAISE_INITIALIZATION_ERROR:
                        return RaiseInitializationError(message);
                    default:
                        return Message.WithEmptyBody((int)CodeStatus.NO_OPERATION,
                                                     (int)OperationCode.NO_OPERATION);
                }
            }
            catch (Exception)
            {
                _Logger.LogCritical("Server network communicator critical error!");
                return Message.WithEmptyBody((int)CodeStatus.ERROR,
                                                     (int)OperationCode.NO_OPERATION);
            }
        }
        private Message GetCurrentTaskMetadata()
        {
            Message metadata = Message.WithEmptyBody((int)CodeStatus.OK,
                (int)OperationCode.NO_OPERATION);
            Tuple<int, LoadedComponent<LegionTask>> current = GetCurrentTask();
            if (current == null)
                return Message.WithEmptyBody((int)CodeStatus.NO_CONTENT, (int)OperationCode.NO_OPERATION);
            metadata.AddPage(BodyPage.FromObject("name", 
                current.Item2.Name + ";" + current.Item2.Version + ";" + 
                current.Item2.Publisher));
            metadata.AddPage(BodyPage.FromObject("id", current.Item1));
            return metadata;
        }
        private Message GetCurrentTaskFiles(Message message)
        {
            LoadedComponent<LegionTask> current
                = _KnownComponents[message.Body.GetPage(0).ToObject<int>()];
                
            return Message.FromFiles(
                current.RequiredAssemblies
                    .Where(x => !BANNED_ASSEMBLIES.Contains(x.Name))
                    .Select(x => x.Name + ".dll"),
                (int)CodeStatus.OK,
                (int)OperationCode.NO_OPERATION);
        }
        public Tuple<int, LoadedComponent<LegionTask>> GetCurrentTask()
        {
            Tuple<int, LoadedComponent<LegionTask>> current = _Server.CurrentTask;
            if (current == null)
                return null;
            lock(_KnownComponents)
            {
                if (!_KnownComponents.ContainsKey(current.Item1))
                    _KnownComponents.Add(current.Item1, current.Item2);
            }
            return current;
        }
        public Message GetDataIn(Message request)
        {
            List<int> tasksIds = request.Body.GetPage(0).ToObject<List<int>>();
            return Message.FromObject("dataIn",
                GetDataIn(tasksIds), (int)CodeStatus.OK, (int)OperationCode.NO_OPERATION);
        }
        public List<LegionDataIn> GetDataIn(List<int> tasks)
        {
            return _Server.GetDataIn(tasks);
        }
        public Message RaiseError(Message request)
        {
            (int TaskId, int ParameterId, Exception exception) error 
                = request.Body.GetPage(0).ToObject<(int TaskId, int ParameterId, Exception exception)>();
            RaiseError(error);
            return Message.WithEmptyBody((int)CodeStatus.OK, (int)OperationCode.NO_OPERATION);
        }
        public void RaiseError((int TaskId, int ParameterId, Exception exception) error)
        {
            _Server.RaiseError(error);
        }
        public Message RaiseInitializationError(Message request)
        {
            Tuple<Exception, int> exceptionTaskId = request.Body.GetPage(0).ToObject<Tuple<Exception, int>>();
            RaiseInitializationError(exceptionTaskId);
            return Message.WithEmptyBody((int)CodeStatus.OK, (int)OperationCode.NO_OPERATION);
        }
        public void RaiseInitializationError(Tuple<Exception, int> exceptionTaskId)
        {
            _Server.RaiseInitializationError(exceptionTaskId);
        }
        public Message SaveResults(Message message)
        {
            List<Tuple<int, LegionDataOut>> dataOut = message.Body.GetPage(0).ToObject<List<Tuple<int, LegionDataOut>>>();
            SaveResults(dataOut);
            return Message.WithEmptyBody((int)CodeStatus.OK, (int)OperationCode.NO_OPERATION);

        }
        public void SaveResults(List<Tuple<int, LegionDataOut>> dataOut)
        {
            _Server.SaveResults(dataOut);
        }
    }
}
