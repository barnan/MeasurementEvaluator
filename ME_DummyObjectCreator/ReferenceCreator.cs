using DataStructures.ReferenceSample;
using Interfaces.BaseClasses;
using Interfaces.DataAcquisition;
using Interfaces.ReferenceSample;
using System.Collections.Generic;
using System.IO;

namespace ME_DummyObjectCreator
{
    internal class ReferenceCreator
    {

        private string _fileExtension = ".Ref";


        internal void Create(string referencePath, IHDDFileReaderWriter readerWriter)
        {
            // ttr sample1:
            ReferenceValue refVal1 = new ReferenceValue("Thickness Average", Units.um, 200);
            ReferenceValue refVal2 = new ReferenceValue("Thickness Std", Units.um, 2);
            ReferenceValue refVal3 = new ReferenceValue("Resistivity Average", Units.Ohmcm, 1.5);

            List<IReferenceValue> referenceValues1 = new List<IReferenceValue> { refVal1, refVal2, refVal3 };
            ReferenceSample referenceSample1 = new ReferenceSample("TTR Reference 01", referenceValues1);
            readerWriter.WriteToFile(referenceSample1, Path.Combine(referencePath, $"TTR_Ref_01{_fileExtension}"));


            // ttr sample2:
            ReferenceValue refVal11 = new ReferenceValue("Thickness Average", Units.um, 190);
            ReferenceValue refVal12 = new ReferenceValue("Thickness Std", Units.um, 2);
            ReferenceValue refVal13 = new ReferenceValue("Resistivity Average", Units.Ohmcm, 1.4);

            List<IReferenceValue> referenceValues11 = new List<IReferenceValue> { refVal11, refVal12, refVal13 };
            ReferenceSample referenceSample11 = new ReferenceSample("TTR Reference 02", referenceValues11);
            readerWriter.WriteToFile(referenceSample11, Path.Combine(referencePath, $"TTR_Ref_02{_fileExtension}"));


            // ttr sample2:
            ReferenceValue refVal21 = new ReferenceValue("Thickness Average", Units.um, 210);
            ReferenceValue refVal22 = new ReferenceValue("Thickness Std", Units.um, 2);
            ReferenceValue refVal23 = new ReferenceValue("Resistivity Average", Units.Ohmcm, 1.6);

            List<IReferenceValue> referenceValues21 = new List<IReferenceValue> { refVal21, refVal22, refVal23 };

            ReferenceSample referenceSample21 = new ReferenceSample("TTR Reference 03", referenceValues21);

            readerWriter.WriteToFile(referenceSample21, Path.Combine(referencePath, $"TTR_Ref_03{_fileExtension}"));

        }
    }
}
