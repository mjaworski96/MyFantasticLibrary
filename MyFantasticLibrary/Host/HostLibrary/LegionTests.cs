using LegionCore.Architecture.Server;
using LegionCore.Architecture.Client;
using LegionCore.InMemoryCommunication;
using LegionCore.Architecture;
using System.Threading.Tasks;
using LegionCore.NetworkCommunication;

namespace HostLibrary
{
    public class LegionTests : ITest
    {

        public void Test()
        {
            TestNetwork();
        }
        public async Task TestServer()
        {
            LegionServer server = LegionServer.StartNew().Item2;
            NetworkServer serverManager = new NetworkServer(server);
            Task serverManagerTask = serverManager.Start();
            await serverManagerTask;
        }
        public async void TestClient()
        {
            IClientCommunicator communicator = new NetworkClient();
            while (! await RunClient(communicator)) ;
        }
        private async void TestNetwork()
        {
            LegionServer server = LegionServer.StartNew().Item2;
            NetworkServer serverManager = new NetworkServer(server);
            Task serverManagerTask = serverManager.Start();
            IClientCommunicator communicator = new NetworkClient();
            while (! await RunClient(communicator));
            await serverManagerTask;
        }
        
        private async void TestInMemory()
        {
            LegionServer server = LegionServer.StartNew().Item2;
            IClientCommunicator communicator = new InMemoryClientCommunicator(server);
            while (! await RunClient(communicator));
        }

        private async Task<bool> RunClient(IClientCommunicator communicator)
        {
            try
            {
                LegionClient client = new LegionClient(communicator);
                await client.Run();
                return true;
            }
            catch (LegionException)
            {
                return false;
            }
        }
    }
}
