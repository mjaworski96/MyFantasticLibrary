using ConfigurationManager;
using System;
using System.Xml;

namespace Logging
{
    /// <summary>
    /// Handler of logger section in app.config.
    /// </summary>
    public class LoggerConfiguration
    {
        /// <summary>
        /// Creates logger from app.config.
        /// Should be used in other class that can acces System.Configuration.IConfigurationSectionHandler in Create method.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext"> Configuration context object.</param>
        /// <param name="section">Section XML node</param>
        /// <returns>Logger implementation.</returns>
        public object CreateLoggers(object parent, object configContext, XmlNode section)
        {
            bool param = false;
            if (section.Attributes["parameter"] != null)
                param = true;
            object logObject;
            if (param)
            {
                logObject =
                 Activator.CreateInstance(Type.GetType(section.Attributes["type"].Value), section.Attributes["parameter"].Value);
            }
            else
            {
                logObject =
                  Activator.CreateInstance(Type.GetType(section.Attributes["type"].Value));
            }
            if (section.Attributes["filter"] != null)
            {
                ((Logger)logObject).Filter = (LogType)Enum.Parse(typeof(LogType), section.Attributes["filter"].Value);
            }
            return logObject;
        }
        /// <summary>
        /// Gets logger from configuration.
        /// </summary>
        /// <param name="config"><see cref="Configuration"/> with logger information.</param>
        /// <returns>Initialized logger.</returns>
        internal static Logger Create(Configuration config)
        {
            string parameter = config.GetString("logger.parameter");
            string type = config.GetString("logger.type");
            string filter = config.GetString("logger.filter");
            Logger logger;
            if (parameter != "")
            {
                logger = (Logger)
                 Activator.CreateInstance(Type.GetType(type), parameter);
            }
            else
            {
                logger = (Logger)
                  Activator.CreateInstance(Type.GetType(type));
            }

            if (filter != "")
            {
                logger.Filter = (LogType)Enum.Parse(typeof(LogType), filter);
            }
            return logger;
        }
    }
}
