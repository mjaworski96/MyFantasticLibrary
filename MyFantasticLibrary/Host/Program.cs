using System;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ITest test = new LoggerTest();
            test.Test();
            Console.ReadKey();
        }
    }
}
