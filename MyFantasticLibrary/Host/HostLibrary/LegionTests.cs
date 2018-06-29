using LegionCore.Architecture;
using LegionCore.InMemoryCommunication;
using System;
using System.Threading;

namespace HostLibrary
{
    public class LegionTests : ITest
    {

        public void Test()
        {
            Server server = Server.StartNew().Item2;
            InMemoryServerManager serverManager = new InMemoryServerManager(server);
            IClientCommunicator communicator = new InMemoryClientCommunicator(serverManager);
            Client client = new Client(communicator);
            client.Init();
            client.Run();


        }
    }
}
