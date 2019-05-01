using Frame.PluginLoader.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Frame.PluginLoader
{
    internal class ComponentList
    {
        internal List<Component> Components { get; private set; }
        private const string NAME_ATTRIBUTE_NAME = "Name";
        private const string ASSEMBLY_ATTRIBUTE_NAME = "Assembly";
        private const string INTERFACE_ATTRIBUTE_NAME = "Interfaces";
        private const string COMPONENT_NODE_NAME = "Component";


        /// <summary>
        /// Loads the compnent name and type list from the received XmlElement
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="inputElement">input xml element</param>
        /// <returns>false -> should be saved (it was empty), true -> save not needed </returns>
        internal bool Load(XElement inputElement)
        {
            Components = new List<Component>();

            if (inputElement.HasChildNodes)
            {
                XmlNode componentsNode = inputElement.FirstChild;

                if (componentsNode != null && componentsNode.HasChildNodes)
                {
                    foreach (XmlNode componentNode in componentsNode.ChildNodes)
                    {
                        string nameText = null;
                        string assemblyText = null;
                        string interfaceText = null;

                        foreach (XmlAttribute attribute in componentNode.Attributes)
                        {
                            if (attribute.Name == NAME_ATTRIBUTE_NAME)
                            {
                                nameText = attribute.InnerText;
                            }

                            if (attribute.Name == ASSEMBLY_ATTRIBUTE_NAME)
                            {
                                assemblyText = attribute.InnerText;
                            }

                            if (attribute.Name == INTERFACE_ATTRIBUTE_NAME)
                            {
                                interfaceText = attribute.InnerText;
                            }
                        }

                        if (!string.IsNullOrEmpty(nameText) && !string.IsNullOrEmpty(interfaceText) && !string.IsNullOrEmpty(assemblyText))
                        {
                            string[] interfaces = interfaceText.Split(';');
                            Components.Add(new Component { Name = nameText, Interfaces = interfaces.ToList(), AssemblyName = assemblyText });
                        }
                    }
                }
            }

            // create dummy component for example:
            if (Components.Count == 0)
            {
                XmlElement dummyelement = xmlDoc.CreateElement("Components");
                XmlElement dummyChildElement = xmlDoc.CreateElement(COMPONENT_NODE_NAME);

                XmlAttribute nameAttribute = xmlDoc.CreateAttribute(NAME_ATTRIBUTE_NAME);
                nameAttribute.InnerText = "MeasurementEvaluator";
                XmlAttribute typeAttribute = xmlDoc.CreateAttribute(ASSEMBLY_ATTRIBUTE_NAME);
                typeAttribute.InnerText = "Measurement EValuator";
                XmlAttribute interfaceAttribute = xmlDoc.CreateAttribute(INTERFACE_ATTRIBUTE_NAME);
                interfaceAttribute.InnerText = nameof(IRunable);

                dummyChildElement.Attributes.Append(nameAttribute);
                dummyChildElement.Attributes.Append(typeAttribute);
                dummyChildElement.Attributes.Append(interfaceAttribute);

                dummyelement.AppendChild(dummyChildElement);
                inputElement.AppendChild(dummyelement);

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
