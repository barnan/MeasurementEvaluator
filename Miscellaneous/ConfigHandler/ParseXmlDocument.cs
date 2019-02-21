using NLog;
using System;
using System.Xml;

namespace Miscellaneous
{
    public class ParseXmlDocument
    {
        private ILogger _logger;


        public bool Parse(object obj, Type type, XmlDocument xmlDocument)
        {
            if (obj == null)
            {
                _logger.Error("Received obj is null.");
                return false;
            }

            if (xmlDocument == null)
            {
                _logger.Error($"Received {nameof(xmlDocument)} is null.");
                return false;
            }

            if (obj.GetType() != type)
            {
                _logger.Error($"Type of the arrived .");
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
