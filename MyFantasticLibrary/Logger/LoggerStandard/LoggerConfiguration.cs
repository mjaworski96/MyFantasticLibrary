using System;
using System.Xml;

namespace Logging
{
    /// <summary>
    /// Handler of logger section in app.config.
    /// </summary>
    class LoggerConfiguration
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
                switch (section.Attributes["filter"].Value)
                {
                    case "Information":
                        ((Logger)logObject).Filter = LogType.Information;
                        break;
                    case "Warning":
                        ((Logger)logObject).Filter = LogType.Warning;
                        break;
                    case "Error":
                        ((Logger)logObject).Filter = LogType.Error;
                        break;
                    default:
                        break;
                }
            }
                return logObject;
        }
    }
}
