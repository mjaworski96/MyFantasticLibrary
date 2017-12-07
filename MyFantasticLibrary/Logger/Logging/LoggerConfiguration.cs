using System;
using System.Configuration;
using System.Xml;

namespace Logging
{
    class LoggerConfiguration: IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
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
            return logObject;
        }
    }
}
