using System;

namespace ComponentContract
{
    /// <summary>
    /// Attribute for exporting components.
    /// </summary>
    public class ComponentAttribute : Attribute
    {
        /// <summary>
        /// Initializes new instance of ComponentAttribute. Other properties are set as "Unknown".
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name is null or empty.</exception>
        /// <param name="name">Name of component</param>
        public ComponentAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter message can not be empty.", nameof(name));
            }

            Name = name;
            Version = "Unknown";
            Publisher = "Unknown";
            Description = "Unknown";
        }
        /// <summary>
        /// Initializes new instance of ComponentAttribute. Other properties are set as "Unknown".
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name is null or empty.</exception>
        /// <param name="name">Name of component.</param>
        /// <param name="version">Version of component.</param>
        public ComponentAttribute(string name, string version) : this(name)
        {
            Version = version;
        }
        /// <summary>
        /// Initializes new instance of ComponentAttribute. Other properties are set as "Unknown".
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name is null or empty.</exception>
        /// <param name="name">Name of component.</param>
        /// <param name="version">Version of component.</param>
        /// <param name="publisher">Publieher of component.</param>
        public ComponentAttribute(string name, string version, string publisher) : this(name, version)
        {
            Publisher = publisher;
        }
        /// <summary>
        /// Initializes new instance of ComponentAttribute. Other properties are set as "Unknown".
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name is null or empty.</exception>
        /// <param name="name">Name of component.</param>
        /// <param name="version">Version of component.</param>
        /// <param name="publisher">Publieher of component.</param>
        /// <param name="description">Description of component.</param>
        public ComponentAttribute(string name, string version, string publisher, string description) : this(name, version, publisher)
        {
            Description = description;
        }

        /// <summary>
        /// Name of component.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Version of component.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Publisher of component.
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// Description of component.
        /// </summary>
        public string Description { get; set; }
    }
}
