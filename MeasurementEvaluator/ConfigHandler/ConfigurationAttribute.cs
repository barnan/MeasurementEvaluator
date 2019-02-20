using System;

namespace MeasurementEvaluator.ConfigHandler
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConfigurationAttribute : Attribute
    {

        public ConfigurationAttribute(string name)
            : this(name, null, false)
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


        private string _name;
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        private string _description;
        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        private bool _loadComponent;
        public bool LoadComponent
        {
            get => _loadComponent;
            private set => _loadComponent = value;
        }

    }
}
