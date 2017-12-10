using ComponentContract;
using System;
using System.Reflection;

namespace ComponentsLoader
{
    public class LoadedComponent<T>
    {
        private ComponentAttribute _attr;
        private Type _component;
        private T _singleton;
        public T Singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = NewInstantion;
                return _singleton;
            }
        }

        public T NewInstantion
        {
            get
            {
                return (T)Activator.CreateInstance(_component);
            }
        }
        

        public LoadedComponent(Type component)
        {
            _component = component;
            _attr = (ComponentAttribute)component
                .GetCustomAttribute(
                typeof(ComponentAttribute));
        }

        public string Name
        {
            get
            {
                return _attr.Name;
            }
        }
        public string Version
        {
            get
            {
                return _attr.Version;
            }
        }
        public string Publisher
        {
            get
            {
                return _attr.Publisher;
            }
        }
        public string Description
        {
            get
            {
                return _attr.Description;
            }
        }
        public override string ToString()
        {
            return Name + " " + Version +
                " " + Publisher + " " + Description;
        }
    }
}
