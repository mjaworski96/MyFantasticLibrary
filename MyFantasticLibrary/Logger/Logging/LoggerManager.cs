using System.Configuration;

namespace Logging
{
    public class LoggerManager
    {
        public static Logger Default { get; }
        public static FileLogger FileLogger { get; }
        public static ConsoleLogger ConsoleLogger { get; }
        static LoggerManager()
        {
            Default = (Logger)ConfigurationManager.GetSection("Logger");
            FileLogger = new FileLogger("log.txt");
            ConsoleLogger = new ConsoleLogger();

            if (Default == null)
                Default = FileLogger;
        }
    }
}
