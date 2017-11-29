using System;
using System.Configuration;
using System.Xml;

namespace Logging
{
    class LoggerConfiguration: IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            //if (section.ChildNodes.Count == 0)
                //return null;
            //XmlNode node = section.ChildNodes.Item(0);
            bool param = false;
            if (section.Attributes["parameter"] != null)
                param = true;
            object plugObject;
            if (param)
            {
                plugObject =
                 Activator.CreateInstance(Type.GetType(section.Attributes["type"].Value), section.Attributes["parameter"].Value);
            }
            else
            {
                plugObject =
                  Activator.CreateInstance(Type.GetType(section.Attributes["type"].Value));
            }
            return plugObject;
        }
    }
}
