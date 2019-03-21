using Frame.PluginLoader.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Frame.PluginLoader
{
    internal class ComponentList
    {
        internal List<KeyValuePair<string, List<string>>> Components { get; private set; }


        /// <summary>
        /// Loads the compnent name and type list from the received XmlElement
        /// </summary>
        /// <param name="inputElement">input xml element</param>
        /// <returns>false -> should be saved (it was empty), true -> save not needed </returns>
        internal bool Load(XmlElement inputElement)
        {
            Components = new List<KeyValuePair<string, List<string>>>();

            if (inputElement.HasChildNodes)
            {
                foreach (XmlElement element in inputElement.ChildNodes)
                {
                    if (!element.HasAttributes)
                    {
                        continue;
                    }

                    foreach (XmlAttribute elementAttribute in element.Attributes)
                    {
                        string name = elementAttribute.Name;
                        string text = elementAttribute.InnerText;

                        string[] types = text.Split();

                        Components.Add(new KeyValuePair<string, List<string>>(name, types.ToList()));
                    }
                }
            }

            if (Components.Count == 0)
            {
                // add a dummy component

                Components.Add(new KeyValuePair<string, List<string>>("MeasurementEvaluator", new List<string> { typeof(IRunable).ToString() }));

                return false;
            }

            return true;
        }

    }
}
