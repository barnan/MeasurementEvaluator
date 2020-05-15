using Frame.PluginLoader.Interfaces;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Frame.PluginLoader
{
    internal class ComponentList
    {
        internal List<Component> Components { get; private set; }
        private const string NAME_ATTRIBUTE_NAME = "Name";
        private const string ASSEMBLY_ATTRIBUTE_NAME = "Assembly";
        private const string INTERFACE_ATTRIBUTE_NAME = "Interfaces";
        private const string COMPONENTS_NODE_NAME = "Components";
        private const string COMPONENT_NODE_NAME = "Component";


        /// <summary>
        /// Loads the compnent name and type list from the received XmlElement
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="inputElement">input xml element</param>
        /// <returns>false -> should be saved (it was empty), true -> save not needed </returns>
        internal bool Load(XElement inputElement, ILogger logger)
        {
            Components = new List<Component>();

            if (inputElement.HasElements)
            {
                XElement componentsElement = inputElement.Element(COMPONENTS_NODE_NAME);

                if (componentsElement != null && componentsElement.HasElements)
                {
                    foreach (XElement componentXElement in componentsElement.Elements())
                    {
                        string nameText = null;
                        string assemblyText = null;
                        string interfaceText = null;

                        foreach (XAttribute attribute in componentXElement.Attributes())
                        {
                            if (attribute.Name == NAME_ATTRIBUTE_NAME)
                            {
                                nameText = attribute.Value;
                            }

                            if (attribute.Name == ASSEMBLY_ATTRIBUTE_NAME)
                            {
                                assemblyText = attribute.Value;
                            }

                            if (attribute.Name == INTERFACE_ATTRIBUTE_NAME)
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
                XElement dummyelement = new XElement(COMPONENTS_NODE_NAME);
                XElement dummyChildElement = new XElement(COMPONENT_NODE_NAME);

                XAttribute nameAttribute = new XAttribute(NAME_ATTRIBUTE_NAME, "MeasurementEvaluator");
                XAttribute typeAttribute = new XAttribute(ASSEMBLY_ATTRIBUTE_NAME, "MeasurementEValuator");
                XAttribute interfaceAttribute = new XAttribute(INTERFACE_ATTRIBUTE_NAME, nameof(IRunable));

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
