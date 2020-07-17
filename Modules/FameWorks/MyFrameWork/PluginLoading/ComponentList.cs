using MyFrameWork.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MyFrameWork.PluginLoading
{
    internal class ComponentList
    {
        internal List<Component> Components { get; private set; }
        private readonly string _NAME_ATTRIBUTE_NAME = "Name";
        private readonly string _ASSEMBLY_ATTRIBUTE_NAME = "Assembly";
        private readonly string _INTERFACE_ATTRIBUTE_NAME = "Interfaces";
        private readonly string _COMPONENTS_NODE_NAME = "Components";
        private readonly string _COMPONENT_NODE_NAME = "Component";

        private readonly string _COMPONENT_SECTION_NAME = "ComponentList";
        private readonly string _COMPONENT_FILE_NAME = "ComponentList";
        private readonly string _CONFIG_FILE_EXTENSION = ".config";


        /// <summary>
        /// Reads the list of available components from the ComponentList.config or creates a dummy component list
        /// </summary>
        /// <returns>returns with the componentlist from the ComponentList.config or a dummy componentList (example)</returns>
        internal ComponentList CreateComponentList(string configurationFolder, ConfigHandler.ConfigHandler configHandler, IMyLogger logger)
        {
            try
            {
                string componentListFileName = Path.Combine(configurationFolder, $"{_COMPONENT_FILE_NAME}{_CONFIG_FILE_EXTENSION}");

                ConfigHandler.ConfigHandler.CreateConfigFileIfNotExisting(componentListFileName);

                ComponentList componentList = new ComponentList();
                XElement componentListSection = ConfigHandler.ConfigHandler.LoadSectionXElementFromFile(componentListFileName, _COMPONENT_SECTION_NAME, typeof(ComponentList));

                if (componentListSection == null)
                {
                    componentListSection = ConfigHandler.ConfigHandler.CreateSectionXElement(_COMPONENT_SECTION_NAME, typeof(ComponentList));
                }

                if (!componentList.Load(componentListSection, logger))
                {
                    ConfigHandler.ConfigHandler.Save(componentListFileName, _COMPONENT_SECTION_NAME, componentListSection, typeof(ComponentList));
                }

                return componentList;
            }
            catch (Exception ex)
            {
                // todo throw stacktrace
                throw new Exception($"Problem during component list loading: {ex.Message}");
            }
        }



        /// <summary>
        /// Loads the compnent name and type list from the received XmlElement
        /// </summary>
        /// <param name="inputElement">input xml element</param>
        /// <param name="logger"></param>
        /// <returns>false -> should be saved (it was empty), true -> save not needed </returns>
        internal bool Load(XElement inputElement, IMyLogger logger)
        {
            Components = new List<Component>();

            if (inputElement.HasElements)
            {
                XElement componentsElement = inputElement.Element(_COMPONENTS_NODE_NAME);

                if (componentsElement != null && componentsElement.HasElements)
                {
                    foreach (XElement componentXElement in componentsElement.Elements())
                    {
                        string nameText = null;
                        string assemblyText = null;
                        string interfaceText = null;

                        foreach (XAttribute attribute in componentXElement.Attributes())
                        {
                            if (attribute.Name == _NAME_ATTRIBUTE_NAME)
                            {
                                nameText = attribute.Value;
                            }

                            if (attribute.Name == _ASSEMBLY_ATTRIBUTE_NAME)
                            {
                                assemblyText = attribute.Value;
                            }

                            if (attribute.Name == _INTERFACE_ATTRIBUTE_NAME)
                            {
                                interfaceText = attribute.Value;
                            }
                        }

                        if (!string.IsNullOrEmpty(nameText) && !string.IsNullOrEmpty(interfaceText) && !string.IsNullOrEmpty(assemblyText))
                        {
                            string[] interfaces = interfaceText.Split(';');
                            Components.Add(new Component { Name = nameText, Interfaces = interfaces.ToList(), AssemblyName = assemblyText });

                            logger.Info($"{nameText} {assemblyText} {string.Join(",", interfaces)} added to the component list.");
                        }
                    }
                }
            }

            // create dummy component for example:
            if (Components.Count == 0)
            {
                XElement dummyelement = new XElement(_COMPONENTS_NODE_NAME);
                XElement dummyChildElement = new XElement(_COMPONENT_NODE_NAME);

                XAttribute nameAttribute = new XAttribute(_NAME_ATTRIBUTE_NAME, "MeasurementEvaluator");
                XAttribute typeAttribute = new XAttribute(_ASSEMBLY_ATTRIBUTE_NAME, "MeasurementEValuator");
                XAttribute interfaceAttribute = new XAttribute(_INTERFACE_ATTRIBUTE_NAME, nameof(IRunable));

                dummyChildElement.Add(nameAttribute);
                dummyChildElement.Add(typeAttribute);
                dummyChildElement.Add(interfaceAttribute);

                dummyelement.Add(dummyChildElement);
                inputElement.Add(dummyelement);

                return false;
            }

            return true;
        }


        internal class Component
        {
            internal string Name { get; set; }
            internal string AssemblyName { get; set; }
            internal List<string> Interfaces { get; set; }
        }
    }
}
