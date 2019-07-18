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

            PairingKeyValuePair ttrThicknessAverage = new PairingKeyValuePair("Thickness Average Avg Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" }, "Thickness Average");
            PairingKeyValuePair ttrThicknessStd = new PairingKeyValuePair("Thickness Average Std Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" }, "Thickness Std");



            List<PairingKeyValuePair> pairList = new List<PairingKeyValuePair>();
            pairList.Add(ttrThicknessAverage);
            pairList.Add(ttrThicknessStd);



            readerWriter.WriteToFile(pairList, Path.Combine(specificationPath, $"DataNamePairs{_fileExtension}"));
        }

    }
}
