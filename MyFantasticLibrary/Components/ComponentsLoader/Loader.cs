using ComponentContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ComponentsLoader
{
    public class Loader
    {
        internal ComponentAttribute GetComponentAttribute(Type componentType)
        {
            return (ComponentAttribute)componentType.GetCustomAttribute(typeof(ComponentAttribute));
        }
        internal ComponentAttribute GetComponentAttribute<T>(T component)
        {
            return GetComponentAttribute(component.GetType());
        }

        public static T Instantialize<T>(Type toInstantialize)
        {
            return (T)Activator.CreateInstance(toInstantialize);
        }
        public static object Instantialize(Type toInstantialize)
        {
            return Activator.CreateInstance(toInstantialize);
        }

        private List<Assembly> LoadAssemblies(string directoryPath = ".")
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

        public List<Type> GetComponentsTypes(string directoryPath = ".")
        {
            List<Type> types = new List<Type>();

            foreach (var assembly in LoadAssemblies(directoryPath))
            {
                try
                {
                    Type[] t = assembly.GetTypes();
                    foreach (Type type in t)
                    {
                        ComponentAttribute attr = GetComponentAttribute(type);
                        if (attr != null)
                        {
                            types.Add(type);
                        }
                    }
                }
                catch (Exception) { }
            }
            return types;
        }
        public List<Type> GetComponentsTypes(string name, string directoryPath = ".")
        {
            return (from component 
                    in GetComponentsTypes(directoryPath)
                    where GetComponentAttribute(component).Name == name
                    select component).ToList();
        }

        public List<Type> GetComponentsTypes<T>(string directoryPath = ".")
        {
            return (from component 
                    in GetComponentsTypes(directoryPath)
                    where component.GetInterfaces().Contains(typeof(T))
                    select component).ToList();
        }
        public List<Type> GetComponentsTypes<T>(string name, string directoryPath = ".")
        {
            return (from component 
                    in GetComponentsTypes<T>(directoryPath)
                    where GetComponentAttribute(component).Name == name
                    select component).ToList();
        }
   
        public List<T> GetComponents<T>(string directoryPath = ".")
        {
            return (from component
                    in GetComponentsTypes<T>(directoryPath)
                    select Instantialize<T>(component)).ToList();
        }
        public List<T> GetComponents<T>(string name, string directoryPath = ".")
        {
            return (from component
                    in GetComponents<T>(directoryPath)
                    where GetComponentAttribute(component).Name == name
                    select component).ToList();
        }

        public Type GetComponentType(string directoryPath)
        {
            return GetComponentsTypes(directoryPath).FirstOrDefault();
        }
        public Type GetComponentType(string name, string directoryPath)
        {
            return GetComponentsTypes(name, directoryPath).FirstOrDefault();
        }

        public Type GetComponentType<T>(string directoryPath)
        {
            return GetComponentsTypes<T>(directoryPath).FirstOrDefault();
        }
        public Type GetComponentType<T>(string name, string directoryPath)
        {
            return GetComponentsTypes<T>(name, directoryPath).FirstOrDefault();
        }

        public T GetComponent<T>(string name, string directoryPath = ".")
        {
            return GetComponents<T>(name, directoryPath).FirstOrDefault();
        }
        public T GetComponent<T>(string directoryPath = ".")
        {
            return GetComponents<T>(directoryPath).FirstOrDefault();
        }
    }
}
