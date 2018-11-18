using System;
using System.Linq;
using System.Xml.Linq;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    class XmlReader : MeasDataFileBase
    {
        public XmlReader(string filename, string toolname) 
            : base(filename, toolname)
        {
        }

        public override IToolMeasurementData ReadFile()
        {
            // TODO: finish xml reading:

            XDocument xml = XDocument.Load(FileName);
            
            // Query the data and write out a subset 
            var query = from c in xml.Root.Descendants("contact") where (int)c.Attribute("id") < 4
                        select c.Element("firstName").Value + " " + c.Element("lastName").Value;

            foreach (string name in query)
            {
                Console.WriteLine("Contact's Full Name: {0}", name);
            }


            return new ToolMeasurementData();

        }
    }
}
