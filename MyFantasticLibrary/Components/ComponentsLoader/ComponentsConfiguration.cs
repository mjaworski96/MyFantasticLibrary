using Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace ComponentsLoader
{
    /// <summary>
    ///  Handler of components section in app.config.
    /// </summary>
    public class ComponentsConfiguration : IConfigurationSectionHandler
    {
        /// <summary>
        /// Reades components configuration from app.config.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext"> Configuration context object.</param>
        /// <param name="section">Section XML node</param>
        /// <returns>List&lt;Tuple&lt;Type, string, string, string, string>> of components parameters.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<Tuple<Type, string, string, string, string>> components = new List<Tuple<Type, string, string, string, string>>();

            foreach (XmlNode node in section.ChildNodes)
            {
                try
                {
                    Type type = Type.GetType(node.Attributes["type"].Value);
                    if (type == null) continue;
                    string name = node.Attributes["name"].Value;
                    if (name == null) continue;
                    string version = node.Attributes["version"]?.Value;
                    string publisher = node.Attributes["publisher"]?.Value;
                    string directory = node.Attributes["directory"]?.Value ?? ".";
                    components.Add(Tuple.Create(type, name, version, publisher, directory));
                }
                catch (Exception e)
                {
                    using (Logger logger = LoggerManager.Default)
                    {
                        logger.Log(LogType.Error, "Error: " + e.Message, true);
                    }
                }
            }

            return components;
        }
    }
}
