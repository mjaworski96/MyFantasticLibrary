using LegionCore.Architecture.Client;
using LegionCore.Logging;
using LegionCore.NetworkCommunication;
using System;
using System.Threading.Tasks;

namespace LegionClientLibrary
{
    public class LegionRunnableClient
    {
        public Task Run()
        {
            LegionClient client = new LegionClient(new NetworkClient());
            return client.Run();
        }
        public static void Main(string[] args)
        {
            try
            {
                new LegionRunnableClient().Run().GetAwaiter().GetResult();
                while (!LoggingManager.Instance.IsQueueEmpty) ;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
