using LegionCore.Architecture;
using LegionCore.InMemoryCommunication;

namespace HostLibrary
{
    public class LegionTests : ITest
    {
        public void Test()
        {
            using (Server server = new Server())
            {
                InMemoryServerManager serverManager = new InMemoryServerManager(server);
                IClientCommunicator communicator = new InMemoryClientCommunicator(serverManager);
                Client client = new Client(communicator);
                client.Init();
                client.Run();
            }
                
        }
    }
}
