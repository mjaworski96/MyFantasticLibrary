using System;

namespace ComponentContract
{
    public class ComponentAttribute : Attribute
    {
        public ComponentAttribute(string name)
        {
            Name = name;
            Version = "Unknown";
            Publisher = "Unknown";
            Description = "Unknown";
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
    }
}
