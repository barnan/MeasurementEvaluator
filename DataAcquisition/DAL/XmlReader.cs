using DataStructures.MeasuredData;
using Interfaces.MeasuredData;
using System;
using System.Linq;
using System.Xml.Linq;

namespace DataAcquisition.DAL
{
    class XmlReader : MeasurementDataFileBase
    {
        public XmlReader(string filename, string toolname)
        {
        }

        public override IToolMeasurementData ReadFile(string filename, string toolname)
        {
            // TODO: finish xml reading:

            XDocument xml = XDocument.Load(filename);

            // Query the data and write out a subset 
            var query = from c in xml.Root.Descendants("contact")
                        where (int)c.Attribute("id") < 4
                        select c.Element("firstName").Value + " " + c.Element("lastName").Value;

            foreach (string name in query)
            {
                Console.WriteLine("Contact's Full Name: {0}", name);
            }


            return new ToolMeasurementData(toolname, null);

        }
    }
}
