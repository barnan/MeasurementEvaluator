using System.Text;
using System.Xml;
using System.Xml.Linq;
using BaseClasses.Repository;
using Interfaces.Misc;
using MeasurementDataStructures;

namespace HDDRespository
{
    internal class HDDRepository : GenericHDDRepository<XElement>
    {
        public HDDRepository(GenericHDDRepositoryParameters parameters) 
            : base(parameters)
        {
        }

        protected override INamedContent<XElement> CreateNamedObjectFromFile(string fileName)
        {
            XElement fileContent = XElement.Load(fileName);

            // it assumes that the XElement has a name attribute!!

            string name = Path.GetFileNameWithoutExtension(fileName);

            return new NamedContent<XElement>
            {
                Name = name,
                Content = fileContent
            };
        }

        protected override void WriteContentToFile(string fileName, XElement content)
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = Encoding.UTF8;
            writerSettings.Indent = true;
            writerSettings.CloseOutput = true;

            using (StreamWriter sw = new StreamWriter(fileName))
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, writerSettings))
            {
                content.Save(xmlWriter);
            }
        }
    }
}