using Logging;

namespace HostLibrary
{
    class LoggerTest : ITest
    {
        public void Test()
        {
            LoggerManager.Config.ConfigFilePath = "config.xml";

            using (Logger logger = LoggerManager.Default)
            {
                logger.Log(LogType.Information, "test", true);
                logger.Log(LogType.Warning, "test", true);
                logger.Log(LogType.Error, "test", true);
            }
            using (Logger consoleLogger = new ConsoleLogger())
            {
                consoleLogger.Log(LogType.Information, "test", true);
                consoleLogger.Log(LogType.Warning, "test", true);
                consoleLogger.Log(LogType.Error, "test", true);
                consoleLogger.Log(LogType.Critical, "test", true);
            }
            using (Logger consoleLogger = new ConsoleLogger("Red"))
            {
                consoleLogger.Log(LogType.Information, "test", true);
                consoleLogger.Log(LogType.Warning, "test", true);
                consoleLogger.Log(LogType.Error, "test", true);
                consoleLogger.Log(LogType.Critical, "test", true);
            }
            using (Logger consoleLogger = new ConsoleLogger("Yellow;Red"))
            {
                consoleLogger.Log(LogType.Information, "test", true);
                consoleLogger.Log(LogType.Warning, "test", true);
                consoleLogger.Log(LogType.Error, "test", true);
                consoleLogger.Log(LogType.Critical, "test", true);
            }
            using (Logger consoleLogger = new ConsoleLogger("Green;Yellow;Red"))
            {
                consoleLogger.Log(LogType.Information, "test", true);
                consoleLogger.Log(LogType.Warning, "test", true);
                consoleLogger.Log(LogType.Error, "test", true);
                consoleLogger.Log(LogType.Critical, "test", true);
            }
            using (Logger consoleLogger = new ConsoleLogger("Green;Yellow;Magenta;Red"))
            {
                consoleLogger.Log(LogType.Information, "test", true);
                consoleLogger.Log(LogType.Warning, "test", true);
                consoleLogger.Log(LogType.Error, "test", true);
                consoleLogger.Log(LogType.Critical, "test", true);
            }
        }
    }
}
