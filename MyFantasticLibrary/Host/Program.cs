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
                //logger.Log(LogType.Information, "xd", true);
                //logger.Log(LogType.Warning, "xd", true);
                //logger.Log(LogType.Error, "xd", true);
            }
            using (Logger consoleLogger = new ConsoleLogger())
            {
                consoleLogger.Log(LogType.Information, "xd", true);
                consoleLogger.Log(LogType.Warning, "xd", true);
                consoleLogger.Log(LogType.Error, "xd", true);
            }
            using (Logger consoleLogger = new ConsoleLogger("Red"))
            {
                consoleLogger.Log(LogType.Information, "xd", true);
                consoleLogger.Log(LogType.Warning, "xd", true);
                consoleLogger.Log(LogType.Error, "xd", true);
            }
            using (Logger consoleLogger = new ConsoleLogger("Yellow;Red"))
            {
                consoleLogger.Log(LogType.Information, "xd", true);
                consoleLogger.Log(LogType.Warning, "xd", true);
                consoleLogger.Log(LogType.Error, "xd", true);
            }
            using (Logger consoleLogger = new ConsoleLogger("Green;Yellow;Red"))
            {
                consoleLogger.Log(LogType.Information, "xd", true);
                consoleLogger.Log(LogType.Warning, "xd", true);
                consoleLogger.Log(LogType.Error, "xd", true);
            }
            Console.ReadKey();
        }
    }
}
