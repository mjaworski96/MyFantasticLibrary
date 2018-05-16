using System;

namespace HostLibrary
{
    public class SharedProgram
    {
        public static void Main(string[] args)
        {
            ITest test = new LegionTests();
            test.Test();
            Console.ReadKey();
        }
    }
}