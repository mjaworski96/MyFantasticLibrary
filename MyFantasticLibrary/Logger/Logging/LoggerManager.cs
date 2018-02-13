using System.Configuration;

namespace Logging
{
    /// <summary>
    /// Manager of loggers;
    /// </summary>
    public class LoggerManager
    {
        /// <summary>
        /// Returns default Logger implementation. This value can be changed from Logger section in app.config.
        /// Default value of logger is FileLogger
        /// Format of this section is:
        /// <logger type="type of logger" parameter="parameter for logger" filter="type of log message"/>
        /// Parameter for logger can be filename or colors of messages.
        /// </summary>
        public static Logger Default { get; }
        /// <summary>
        /// Returns implementation of FileLogger.
        /// </summary>
        public static FileLogger FileLogger { get; }
        /// <summary>
        /// Returns implemention of ConsoleLogger.
        /// </summary>
        public static ConsoleLogger ConsoleLogger { get; }
        /// <summary>
        /// Default constructor.
        /// </summary>
        static LoggerManager()
        {
            Default = (Logger)ConfigurationManager.GetSection("Logger");
            FileLogger = new FileLogger();
            ConsoleLogger = new ConsoleLogger();

            if (Default == null)
                Default = FileLogger;
        }
    }
}
