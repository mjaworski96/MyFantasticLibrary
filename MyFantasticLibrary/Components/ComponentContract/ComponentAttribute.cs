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
        /// <param name="name"></param>
        public ComponentAttribute(string name)
        {
            Name = name;
            Version = "Unknown";
            Publisher = "Unknown";
            Description = "Unknown";
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
