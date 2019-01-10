using System;
using System.Threading.Tasks;

namespace HostLibrary
{
    public class SharedProgram
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    if (args[0] == "client")
                        new LegionTests().TestClient();
                    if (args[0] == "server")
                        Task.WaitAll(new LegionTests().TestServer());
                }
                else
                {
                    ITest test = new LegionTests();
                    test.Test();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            Console.WriteLine("=== THE END ===");
            Console.ReadKey();
        }
    }
}