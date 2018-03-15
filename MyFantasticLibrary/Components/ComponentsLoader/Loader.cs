using ComponentContract;
using ConfigurationManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ComponentsLoader
{
    /// <summary>
    /// Loader of components.
    /// </summary>
    public static class Loader
    {
        /// <summary>
        /// Gets <see cref="ComponentAttribute"/> of component.
        /// </summary>
        /// <param name="componentType">Type of component.</param>
        /// <returns><see cref="ComponentAttribute"/> of component.</returns>
        internal static ComponentAttribute GetComponentAttribute(Type componentType)
        {
            return (ComponentAttribute)componentType.GetCustomAttribute(typeof(ComponentAttribute));
        }
        /// <summary>
        /// Creates instance of component.
        /// </summary>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="toInstantialize">Type to instantialize.</param>
        /// <returns>Instantialized component.</returns>
        public static T Instantialize<T>(Type toInstantialize)
        {
            return (T)Activator.CreateInstance(toInstantialize);
        }
        /// <summary>
        /// Loads all assemblies from selected directory.
        /// </summary>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns>List of assemblies.</returns>
        private static List<Assembly> LoadAssemblies(string directoryPath = ".")
        {
            List<Assembly> assemblies = new List<Assembly>();
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            ComponentAttribute test = new ComponentAttribute("elo");
            FileInfo[] fi = di.GetFiles("*.dll");
            foreach (var item in fi)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(item.FullName);
                    assemblies.Add(assembly);
                }
                catch (Exception) { }
            }
            return assemblies;
        }
        /// <summary>
        /// Get component idetified by name.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns><see cref="LoadedComponent{T}"/> with selected component or null.</returns>
        public static LoadedComponent<T> GetComponentByName<T>(string name, string directoryPath = ".")
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter name can not be null or empty", nameof(name));
            }


            return (from c
                   in GetComponents<T>(directoryPath)
                    where c.Name == name
                    select c).FirstOrDefault();
        }
        /// <summary>
        /// Get components from directory.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns>List of all components in directory.</returns>
        public static List<LoadedComponent<T>> GetComponents<T>(string directoryPath = ".")
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Parameter directoryName can not be null or empty", nameof(directoryPath));
            }
            List<LoadedComponent<T>> components = new List<LoadedComponent<T>>();
            foreach (var assembly in LoadAssemblies(directoryPath))
            {
                try
                {
                    List<Type> types = (from t
                                in assembly.GetTypes()
                                where t.GetInterfaces().Contains(typeof(T)) ||
                                t.BaseType == typeof(T)
                                select t).ToList();

                    foreach (Type type in types)
                    {
                        ComponentAttribute attr = GetComponentAttribute(type);
                        if (attr != null)
                        {
                            components.Add(new LoadedComponent<T>(type));
                        }
                    }
                }
                catch (Exception) { }
            }
            return components;
        }
        /// <summary>
        /// Gets components idetified by name and version.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="version">Version of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns><see cref="LoadedComponent{T}"/> with selected component or null.</returns>
        public static LoadedComponent<T> GetComponentByNameVersion<T>(string name, string version, string directoryPath = ".")
        {
            return (from c
                    in GetComponents<T>(directoryPath)
                    where c.Name == name && c.Version == version
                    select c).FirstOrDefault();
        }
        /// <summary>
        /// Gets components idetified by name and publiser.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="publisher">Publisher of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns><see cref="LoadedComponent{T}"/> with selected component or null.</returns>
        public static LoadedComponent<T> GetComponentByNamePublisher<T>(string name, string publisher, string directoryPath = ".")
        {
            return (from c
                   in GetComponents<T>(directoryPath)
                    where c.Name == name && c.Publisher == publisher
                    select c).FirstOrDefault();
        }
        /// <summary>
        /// Gets components idetified by name, version and publisher.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="version">Version of component.</param>
        /// /// <param name="publisher">Publisher of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns><see cref="LoadedComponent{T}"/> with selected component or null.</returns>
        public static LoadedComponent<T> GetComponentByNameVersionPublisher<T>(string name, string version, string publisher, string directoryPath = ".")
        {
            return (from c
                   in GetComponents<T>(directoryPath)
                    where c.Name == name && c.Version == version &&
                    c.Publisher == publisher
                    select c).FirstOrDefault();
        }
        /// <summary>
        /// Gets components defined in app.config. Structure of components section:
        /// <components>
        ///<component type = "Full name of component base type or interface, Assembly with this type or interface" name="Name of component" version="Version of component" publisher="Publisher of component"/>
        /// ...
        ///</components>
        /// Version and publiser are optional.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown ifconfigPath is null or empty.</exception>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns>List of <see cref="LoadedComponent{T}"/> from configuration.</returns>
        public static List<LoadedComponent<T>> GetComponentsFromConfiguration<T>(string configPath = "config.cfg")
        {
            List<LoadedComponent<T>> components = new List<LoadedComponent<T>>();
            List<Tuple<Type, string, string, string, string>> config = ComponentsConfiguration.Create(
                new Configuration(configPath));

            foreach (var item in config)
            {
                if(item.Item1 == typeof(T))
                {
                    if(item.Item3 == null && item.Item4 == null)
                    {
                        components.Add(
                            GetComponentByName<T>(
                                item.Item2, item.Item5));
                    }
                    else if (item.Item3 != null && item.Item4 == null)
                    {
                        components.Add(
                            GetComponentByNameVersion<T>(
                                item.Item2, item.Item3, item.Item5));
                    }
                    else if (item.Item3 == null && item.Item4 != null)
                    {
                        components.Add(
                            GetComponentByNamePublisher<T>(
                                item.Item2, item.Item4, item.Item5));
                    }
                    else if (item.Item3 != null && item.Item4 != null)
                    {
                        components.Add(
                            GetComponentByNameVersionPublisher<T>(
                                item.Item2, item.Item3, item.Item4, item.Item5));
                    }
                }
            }
            components.RemoveAll(x => x == null);
            return components;
        }
        /// <summary>
        /// Checks if component identified by name is avaiable.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns>Availability of selected component.</returns>
        public static bool IsComponentAvaiableByName<T>(string name, string directoryPath = ".")
        {
            return GetComponentByName<T>(name, directoryPath) != null;
        }
        /// <summary>
        /// Checks if component identified by name and version is avaiable.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="version">Version of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns>Availability of selected component.</returns>
        public static bool IsComponentAvaiableByNameVersion<T>(string name, string version, string directoryPath = ".")
        {
            return GetComponentByNameVersion<T>(name, version, directoryPath) != null;
        }
        /// <summary>
        /// Checks if component identified by name and publisher is avaiable.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="publisher">Publisher of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns>Availability of selected component.</returns>
        public static bool IsComponentAvaiableByNamePublisher<T>(string name, string publisher, string directoryPath = ".")
        {
            return GetComponentByNamePublisher<T>(name, publisher, directoryPath) != null;
        }
        /// <summary>
        /// Checks if component identified by name, version and publisher is avaiable.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name or directoryPath is null or empty.</exception>
        /// <typeparam name="T">Base type or instance of component.</typeparam>
        /// <param name="name">Name of component.</param>
        /// <param name="version">Version of component.</param>
        /// <param name="publisher">Publisher of component.</param>
        /// <param name="directoryPath">Name of directory with components.</param>
        /// <returns>Availability of selected component.</returns>
        public static bool IsComponentAvaiableByNameVersionPublisher<T>(string name, string version, string publisher, string directoryPath = ".")
        {
            return GetComponentByNameVersionPublisher<T>(name, version, publisher, directoryPath) != null;
        }
    }
}
