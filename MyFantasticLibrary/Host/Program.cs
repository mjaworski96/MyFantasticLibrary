using System;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ITest test = new ConfigurationTest();
            test.Test();
            Console.ReadKey();
        }
    }
}
