using Logging;
using System;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ITest test = new ComponentsTest();
            test.Test();
            Console.ReadKey();
        }
    }
}
