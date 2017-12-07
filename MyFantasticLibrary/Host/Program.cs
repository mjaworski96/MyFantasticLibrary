using Logging;
using System;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Logger logger = LoggerManager.Default)
            {
                logger.Log(LogType.Information, "xd", true);
                logger.Log(LogType.Warning, "xd", true);
                logger.Log(LogType.Error, "xd", true);
            }
                
            Console.ReadKey();
        }
    }
}
