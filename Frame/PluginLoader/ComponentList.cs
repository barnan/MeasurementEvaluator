using Frame.PluginLoader.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Frame.PluginLoader
{
    internal class ComponentList
    {
        internal Dictionary<string, List<string>> Components { get; private set; }
        private const string NAME_ATTRIBUTE_NAME = "Name";
        private const string TYPE_ATTRIBUTE_NAME = "Type";
        private const string COMPONENT_NODE_NAME = "Component";


        /// <summary>
        /// Loads the compnent name and type list from the received XmlElement
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="inputElement">input xml element</param>
        /// <returns>false -> should be saved (it was empty), true -> save not needed </returns>
        internal bool Load(XmlDocument xmlDoc, XmlElement inputElement)
        {
            Components = new Dictionary<string, List<string>>();

            if (inputElement.HasChildNodes)
            {
                XmlNode componentsNode = inputElement.FirstChild;

                if (componentsNode != null && componentsNode.HasChildNodes)
                {
                    foreach (XmlNode componentNode in componentsNode.ChildNodes)
                    {
                        string nameText = null;
                        string typeText = null;

                        foreach (XmlAttribute attribute in componentNode.Attributes)
                        {
                            if (attribute.Name == NAME_ATTRIBUTE_NAME)
                            {
                                nameText = attribute.InnerText;
                            }

                            if (attribute.Name == TYPE_ATTRIBUTE_NAME)
                            {
                                typeText = attribute.InnerText;
                            }
                        }

                        if (!string.IsNullOrEmpty(nameText) || !string.IsNullOrEmpty(typeText))
                        {
                            string[] types = typeText.Split(';');
                            Components.Add(nameText, types.ToList());
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
                XmlAttribute typeAttribute = xmlDoc.CreateAttribute(TYPE_ATTRIBUTE_NAME);
                typeAttribute.InnerText = nameof(IRunable);

                dummyChildElement.Attributes.Append(nameAttribute);
                dummyChildElement.Attributes.Append(typeAttribute);

                dummyelement.AppendChild(dummyChildElement);
                inputElement.AppendChild(dummyelement);

                return false;
            }

            return true;
        }
    }
}
