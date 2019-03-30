using System;

namespace Frame.ConfigHandler
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConfigurationAttribute : Attribute
    {
        public ConfigurationAttribute(string description, string name)
            : this(name, description, false)
        {
        }

        public ConfigurationAttribute(string description, string name, bool loadComponent)
        {
            Name = name;
            Description = description;
            LoadComponent = loadComponent;
        }

        public string Name;

        public string Value;

        public string Description;

        public bool LoadComponent;
    }
}
