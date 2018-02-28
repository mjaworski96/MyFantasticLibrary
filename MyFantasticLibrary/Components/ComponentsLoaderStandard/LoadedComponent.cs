using ComponentContract;
using System;
using System.Reflection;

namespace ComponentsLoader
{
    /// <summary>
    /// Represents loaded component.
    /// </summary>
    /// <typeparam name="T">Base type or interface of component.</typeparam>
    public class LoadedComponent<T>
    {
        /// <summary>
        /// ComponenetAttribute of component.
        /// </summary>
        private ComponentAttribute _attr;
        /// <summary>
        /// Type of component.
        /// </summary>
        private Type _component;
        /// <summary>
        /// Singleton instance of component.
        /// </summary>
        private T _singleton;
        /// <summary>
        /// Name of component's assembly.
        /// </summary>
        private AssemblyName _assemblyName;
        /// <summary>
        /// Names of references assemblies to component's assembly.
        /// </summary>
        private AssemblyName[] _referencesAssembliesNames;

        /// <summary>
        /// Singleton instance of component.
        /// </summary>
        public T Singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = NewInstantion;
                return _singleton;
            }
        }
        /// <summary>
        /// New instance of component.
        /// </summary>
        public T NewInstantion
        {
            get
            {
                return (T)Activator.CreateInstance(_component);
            }
        }

        /// <summary>
        /// Initializes new instance of LoadedComponent.
        /// </summary>
        /// <param name="component">Type of component</param>
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
        /// <summary>
        /// Name of component.
        /// </summary>
        public string Name { get => _attr.Name; }
        /// <summary>
        /// Version of component.
        /// </summary>
        public string Version { get => _attr.Version;  }
        /// <summary>
        /// Publisher of component.
        /// </summary>
        public string Publisher {  get => _attr.Publisher; }
        /// <summary>
        /// Description of component.
        /// </summary>
        public string Description { get => _attr.Description; }
        /// <summary>
        /// Name of component's assembly.
        /// </summary>
        public AssemblyName AssemblyName { get => _assemblyName; }
        /// <summary>
        /// Names of references assemblies to component's assembly.
        /// </summary>
        public AssemblyName[] ReferencesAssembliesNames  { get => _referencesAssembliesNames; }
        /// <summary>
        /// ToString() method.
        /// </summary>
        /// <returns>Name, version, publisher and description of component.</returns>
        public override string ToString()
        {
            return Name + " " + Version +
                " " + Publisher + " " + Description;
        }
    }
}
