using System;

namespace MyFrameWork.ConfigHandler
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ConfigurationAttribute : Attribute
    {
        public ConfigurationAttribute(string name)
            : this(string.Empty, name, false)
        {
        }


        public ConfigurationAttribute(string description, string name, bool loadComponent = false)
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
