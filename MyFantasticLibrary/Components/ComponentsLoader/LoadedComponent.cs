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
        private AssemblyName[] _referencedAssembliesNames;

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
        /// <exception cref="ArgumentNullException">Thrown if component is null.</exception>
        /// <param name="component">Type of component</param>
        public LoadedComponent(Type component)
        {
            _component = component ?? throw new ArgumentNullException(nameof(component));
            _attr = (ComponentAttribute)component
                .GetCustomAttribute(
                typeof(ComponentAttribute));
            if (_attr == null)
                throw new NotComponentTypeException();
            _assemblyName = component.Assembly.GetName();
            _referencedAssembliesNames = component.Assembly.GetReferencedAssemblies();
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
        /// Names of referenced assemblies to component's assembly.
        /// </summary>
        public AssemblyName[] ReferencedAssembliesNames  { get => _referencedAssembliesNames; }
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
