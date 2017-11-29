using System.Configuration;

namespace Logging
{
    public class LoggerManager
    {
        public static Logger Default { get; }
        public static FileLogger FileLogger { get; }
        static LoggerManager()
        {
            Default = (Logger)ConfigurationManager.GetSection("Logger");
            FileLogger = new FileLogger("log.txt");

            if (Default == null)
                Default = FileLogger;
        }
    }
}
