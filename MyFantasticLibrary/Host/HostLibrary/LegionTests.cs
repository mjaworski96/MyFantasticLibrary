using LegionCore.Architecture.Server;
using LegionCore.Architecture.Client;
using LegionCore.InMemoryCommunication;
using LegionCore.Architecture;
using System.Threading.Tasks;
using LegionCore.NetworkCommunication;
using System;

namespace HostLibrary
{
    public class LegionTests : ITest
    {

        public void Test()
        {
             Task.WaitAll(TestNetwork());
        }
        private async Task TestNetwork()
        {
            LegionServer server = LegionServer.StartNew().Item2;
            NetworkServer serverManager = new NetworkServer(server);
            Task serverManagerTask = serverManager.Start();
            IClientCommunicator communicator = new NetworkClient();
            await RunClient(communicator);
            await serverManagerTask;
        }
        
        private async void TestInMemory()
        {
            Tuple<Task, LegionServer> server = LegionServer.StartNew();
            IClientCommunicator communicator = new InMemoryClientCommunicator(server.Item2);
            await RunClient(communicator);
            await server.Item1;
        }

        private async Task RunClient(IClientCommunicator communicator)
        {
            LegionClient client = new LegionClient(communicator);
            await client.Run();
        }
    }
}
