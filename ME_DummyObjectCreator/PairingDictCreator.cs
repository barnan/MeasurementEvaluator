using Interfaces.DataAcquisition;
using MeasurementEvaluator.ME_Matching;
using System.Collections.Generic;
using System.IO;

namespace ME_DummyObjectCreator
{
    internal class PairingDictCreator
    {
        private string _fileExtension = ".pair";

        internal void Create(string specificationPath, IHDDFileReaderWriter readerWriter)
        {

            PairingKeyValuePair ttrThicknessStdAbs = new PairingKeyValuePair("Thickness Average Std Abs Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" }, "Thickness Average");
            PairingKeyValuePair ttrThicknessStdRel = new PairingKeyValuePair("Thickness Average Std Rel Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" }, "Thickness Average");



            List<PairingKeyValuePair> pairList = new List<PairingKeyValuePair>();
            pairList.Add(ttrThicknessStdAbs);
            pairList.Add(ttrThicknessStdRel);



            readerWriter.WriteToFile(pairList, Path.Combine(specificationPath, $"DataNamePairs{_fileExtension}"));
        }

    }
}
