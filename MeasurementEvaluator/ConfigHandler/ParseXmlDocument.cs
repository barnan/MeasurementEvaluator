using System;
using System.Xml;

namespace MeasurementEvaluator.ConfigHandler
{
    public class ParseXmlDocument
    {

        public bool Parse(object obj, Type type, XmlDocument xmlDocument)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != type)
            {
                return false;
            }

            XmlNodeList sectionNodes = xmlDocument.DocumentElement.ChildNodes;
            foreach (var section in sectionNodes)
            {

            }


            return true;
        }


    }
}
