using LegionCore.Architecture.Client;
using LegionCore.NetworkCommunication;
using System;
using System.Threading.Tasks;

namespace LegionClientLibrary
{
    public class LegionRunnableClient
    {
        public Task Run()
        {
            return Task.Run(() =>
            {
                LegionClient client = new LegionClient(new NetworkClient());
                client.Init();
                client.Run();
            });
        }
        public static void Main(string[] args)
        {
            new LegionRunnableClient().Run().GetAwaiter().GetResult();
        }
    }
}
