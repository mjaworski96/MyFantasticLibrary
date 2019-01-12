using ComponentContract;
using System;
using System.Collections.Generic;
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
        /// Used for singleton sync.
        /// </summary>
        private static object _SyncRoot = new Object();
        /// <summary>
        /// ComponenetAttribute of component.
        /// </summary>
        private ComponentAttribute _Attr;
        /// <summary>
        /// Type of component.
        /// </summary>
        private Type _Component;
        /// <summary>
        /// Singleton instance of component.
        /// </summary>
        private T _Singleton;
        /// <summary>
        /// Name of component's assembly.
        /// </summary>
        private AssemblyName _AssemblyName;
        /// <summary>
        /// Names of references assemblies to component's assembly.
        /// </summary>
        private AssemblyName[] _ReferencedAssembliesNames;

        /// <summary>
        /// Singleton instance of component.
        /// </summary>
        public T Singleton
        {
            get
            {
                if (_Singleton == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Singleton == null)
                            _Singleton = NewInstantion;
                    }
                }
                return _Singleton;
            }
        }
        /// <summary>
        /// New instance of component.
        /// </summary>
        public T NewInstantion
        {
            get
            {
                return (T)Activator.CreateInstance(_Component);
            }
        }

        /// <summary>
        /// Initializes new instance of LoadedComponent.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if component is null.</exception>
        /// <param name="component">Type of component</param>
        public LoadedComponent(Type component)
        {
            _Component = component ?? throw new ArgumentNullException(nameof(component));
            _Attr = (ComponentAttribute)component
                .GetCustomAttribute(
                typeof(ComponentAttribute));
            if (_Attr == null)
                throw new NotComponentTypeException();
            _AssemblyName = component.Assembly.GetName();
            _ReferencedAssembliesNames = component.Assembly.GetReferencedAssemblies();
        }
        /// <summary>
        /// Name of component.
        /// </summary>
        public string Name { get => _Attr.Name; }
        /// <summary>
        /// Version of component.
        /// </summary>
        public string Version { get => _Attr.Version;  }
        /// <summary>
        /// Publisher of component.
        /// </summary>
        public string Publisher {  get => _Attr.Publisher; }
        /// <summary>
        /// Description of component.
        /// </summary>
        public string Description { get => _Attr.Description; }
        /// <summary>
        /// Name of component's assembly.
        /// </summary>
        public AssemblyName AssemblyName { get => _AssemblyName; }
        /// <summary>
        /// Names of referenced assemblies to component's assembly.
        /// </summary>
        public AssemblyName[] ReferencedAssembliesNames  { get => _ReferencedAssembliesNames; }
        /// <summary>
        /// Name of component's assembly and names of referenced assemblies to component's assembly.
        /// </summary>
        public IEnumerable<AssemblyName> RequiredAssemblies
        {
            get
            {
                yield return AssemblyName;
                foreach (var referenced in ReferencedAssembliesNames)
                {
                    yield return referenced;
                }
            }
        }
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
