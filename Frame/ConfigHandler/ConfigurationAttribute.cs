using System;

namespace Frame.ConfigHandler
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConfigurationAttribute : Attribute
    {

        public ConfigurationAttribute(string description)
        : this(null, description, false)
        {
        }

        public ConfigurationAttribute(string name, string description)
            : this(name, description, false)
        {
        }

        public ConfigurationAttribute(string name, string description, bool loadComponent)
        {
            Name = name;
            Description = description;
            LoadComponent = loadComponent;
        }

        public string Name;

        private string Description;

        public bool LoadComponent;

    }
}
