using DataStructures.ReferenceSample;
using Interfaces.DataAcquisition;
using Interfaces.ReferenceSample;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ME_DummyObjectCreator
{
    internal class ReferenceCreator
    {

        private string _fileExtension = ".Ref";


        internal void Create(string referencePath, IHDDFileReaderWriter readerWriter)
        {
            ReferenceValue refVal1 = new ReferenceValue("Thickness Average", Interfaces.Units.um, 200);
            ReferenceValue refVal2 = new ReferenceValue("Resistivity Average", Interfaces.Units.Ohmcm, 1.5);

            List<IReferenceValue> referenceValues = new List<IReferenceValue> { refVal1, refVal2 };

            ReferenceSample referenceSample1 = new ReferenceSample("Thisckness Reference 500", referenceValues);

            XElement referenceElement = new XElement(nameof(ReferenceSample));
            XElement output = referenceSample1.SaveToXml(referenceElement);

            output.Save(Path.Combine(referencePath, $"TTR_Spec_1{_fileExtension}"));

        }
    }
}
