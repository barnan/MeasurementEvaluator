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

            foreach (var item in xmlDocument.ChildNodes)
            {

            }


            return true;
        }


    }
}
