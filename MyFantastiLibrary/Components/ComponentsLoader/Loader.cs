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
        public List<Assembly> assemblies = new List<Assembly>();
        public List<Type> GetTypes(string directoryPath = ".")
        {
            List<Type> types = new List<Type>();
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            ComponentAttribute test = new ComponentAttribute("elo");
            FileInfo[] fi = di.GetFiles("*.dll");
            foreach (var item in fi)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(item.FullName);
                    assemblies.Add(assembly);
                    Type[] t = assembly.GetTypes();
                    foreach (Type item2 in t)
                    {
                        if(item2.GetCustomAttribute(typeof(ComponentAttribute)) != null)
                        {
                            types.Add(item2);
                        }
                    }
                }
                catch (Exception) { }
            }
            return types;
        }


        public T GetComponents<T>(string name, string directoryPath = ".")
        {
            IEnumerable<Type> avaiableTypes = GetTypes();
            IEnumerable<Type> implements = from inter in avaiableTypes
                                           where inter.GetInterfaces().Contains(typeof(T))
                                           select inter;
            Type type = null;
            foreach (Type componentT in implements)
            {
                T compo = (T)Activator.CreateInstance(componentT);
                ComponentAttribute attr = (ComponentAttribute)componentT.GetCustomAttribute(typeof(ComponentAttribute));
                if (attr.Name == name)
                {
                    type = componentT;
                }
            }
            if (type == null) return default(T);
            T obj = (T)Activator.CreateInstance(type);
            return obj;
        }
    }
}
