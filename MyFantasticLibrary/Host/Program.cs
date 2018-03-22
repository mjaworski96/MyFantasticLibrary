using System;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ITest test = new LegionTests();
            test.Test();
            Console.ReadKey();
        }
    }
}
