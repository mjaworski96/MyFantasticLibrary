using ComponentContract;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ComponentsLoader
{
    public class LoadedComponent<T>
    {
        private ComponentAttribute _attr;
        private Type _component;
        private T _singleton;

        private AssemblyName _assemblyName;
        private AssemblyName[] _referencesAssembliesNames;

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
            if (_attr == null)
                throw new NotComponentTypeException();
            _assemblyName = component.Assembly.GetName();
            _referencesAssembliesNames = component.Assembly.GetReferencedAssemblies();
        }

        public string Name { get => _attr.Name; }
        public string Version { get => _attr.Version;  }
        public string Publisher {  get => _attr.Publisher; }
        public string Description { get => _attr.Description; }

        public AssemblyName AssemblyName { get => _assemblyName; }
        public AssemblyName[] ReferencesAssembliesNames  { get => _referencesAssembliesNames; }

        public override string ToString()
        {
            return Name + " " + Version +
                " " + Publisher + " " + Description;
        }
    }
}
