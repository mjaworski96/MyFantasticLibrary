using ConfigurationManager;
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
    public class ComponentsConfiguration
    {
        /// <summary>
        /// Reades components configuration from app.config.
        ///  Should be used in other class that can acces System.Configuration.IConfigurationSectionHandler in Create method.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext"> Configuration context object.</param>
        /// <param name="section">Section XML node</param>
        /// <returns>List&lt;Tuple&lt;Type, string, string, string, string>> with components parameters.</returns>
        public object CreateComponents(object parent, object configContext, XmlNode section)
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
        /// <summary>
        /// Creates information about components from configuration.
        /// </summary>
        /// <param name="components"><see cref="List{T}"/> of <see cref="Field"/> with information about components.</param>
        /// <returns>List&lt;Tuple&lt;Type, string, string, string, string>>  with components parameters.</returns>
        internal static List<Tuple<Type, string, string, string, string>> Create(List<Field> components)
        {
            List<Tuple<Type, string, string, string, string>> componentsInfo = new List<Tuple<Type, string, string, string, string>>();
           
            foreach (Field component in components)
            {
                Type type = Type.GetType(component.GetField("type").Value);
                if (type == null) continue;
                string name = component.GetField("name").Value;
                if (name == null) continue;
                string version = component.GetField("version")?.Value;
                string publisher = component.GetField("publisher")?.Value;
                string directory = component.GetField("directory")?.Value ?? ".";
                componentsInfo.Add(Tuple.Create(type, name, version, publisher, directory));
            }

            return componentsInfo;
        }
    }
}
