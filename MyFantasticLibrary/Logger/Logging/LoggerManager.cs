using System.Configuration;

namespace Logging
{
    /// <summary>
    /// Manager of loggers.
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
        /// Instance of FileLogger.
        /// </summary>
        public static FileLogger FileLogger { get; }
        /// <summary>
        /// Instance of ConsoleLogger.
        /// </summary>
        public static ConsoleLogger ConsoleLogger { get; }
        /// <summary>
        /// Instance of NullLogger.
        /// </summary>
        public static NullLogger NullLogger { get; }
        /// <summary>
        /// Initializes static parameters of LoggerManager.
        /// </summary>
        static LoggerManager()
        {
            Default = (Logger)ConfigurationManager.GetSection("Logger");
            FileLogger = new FileLogger();
            ConsoleLogger = new ConsoleLogger();
            NullLogger = new NullLogger();

            if (Default == null)
                Default = NullLogger;
        }
    }
}
