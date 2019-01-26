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
                ITest test = new LegionTests();
                test.Test();
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