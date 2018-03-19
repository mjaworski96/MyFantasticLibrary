using ConfigurationManager;

namespace Logging
{
    /// <summary>
    /// Manager of loggers.
    /// </summary>
    public class LoggerManager
    {   
        /// <summary>
        /// Configuration of logger manager.
        /// </summary>
        public static class Config
        {
            /// <summary>
            /// Path to configuration file. Must be set before first use of DefaultLogger.
            /// Defaut value is config.cfg.
            /// </summary>
            public static string ConfigFilePath { get; set; } = "config.cfg";
        }

        /// <summary>
        /// Returns default Logger implementation. This value can be changed from Logger section in app.config.
        /// Not working in .NET Standard Library.
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
            Configuration config = new Configuration(Config.ConfigFilePath);
            Default = LoggerConfiguration.Create(config);
            FileLogger = new FileLogger();
            ConsoleLogger = new ConsoleLogger();
            NullLogger = new NullLogger();

            if (Default == null)
                Default = NullLogger;
        }
    }
}
