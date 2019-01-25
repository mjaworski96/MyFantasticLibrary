using LegionCore.Architecture.Server;
using LegionCore.Logging;
using LegionCore.NetworkCommunication;
using System;
using System.Threading.Tasks;

namespace LegionServerLibrary
{
    public class LegionRunnableServer
    {
        public Task Run()
        {
            LegionServer server = LegionServer.StartNew().Item2;
            NetworkServer serverManager = new NetworkServer(server);
            Task serverManagerTask = serverManager.Start();
            return serverManagerTask;
        }

        public static void Main(string[] args)
        {
            try
            {
                new LegionRunnableServer().Run().GetAwaiter().GetResult();
                while (!LoggingManager.Instance.IsQueueEmpty) ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
