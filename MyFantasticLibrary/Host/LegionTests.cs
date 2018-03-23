using LegionCore.Architecture;
using LegionCore.InMemoryCommunication;

namespace Host
{
    public class LegionTests : ITest
    {
        public void Test()
        {
            using (Server server = new Server())
            {
                InMemoryServerManager serverManager = new InMemoryServerManager(server);
                IClientCommunicator communicator = new InMemoryClientCommunicator(serverManager);
                Client client = new Client(communicator, 2);
                client.Init();
                client.Run();
            }
                
        }
    }
}
