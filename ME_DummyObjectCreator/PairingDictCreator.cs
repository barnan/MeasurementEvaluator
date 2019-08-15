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

            PairingKeyValuePair thicknessAverageAbs = new PairingKeyValuePair("Thickness Average Abs Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" }, "Thickness Average");
            PairingKeyValuePair thicknessAverageRel = new PairingKeyValuePair("Thickness Average Rel Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" });

            PairingKeyValuePair thicknessStdAbs = new PairingKeyValuePair("Thickness Std Abs Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" });
            PairingKeyValuePair thicknessStdRel = new PairingKeyValuePair("Thickness Std Rel Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" });

            PairingKeyValuePair ttvStdAbs = new PairingKeyValuePair("TTV Std Abs Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" });
            PairingKeyValuePair sawmarkStdAbs = new PairingKeyValuePair("Sawmark Std Rel Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" });

            PairingKeyValuePair resistivityStdAbs = new PairingKeyValuePair("Thickness Std Abs Condition", new List<string> { "Resistivity[Ohmcm]", "Resistivity[Î©cm]" }, "Resistivity");
            PairingKeyValuePair resistivityStdRel = new PairingKeyValuePair("Thickness Std Rel Condition", new List<string> { "Resistivity[Ohmcm]", "Resistivity[Î©cm]" }, "Resistivity");



            List<PairingKeyValuePair> pairList = new List<PairingKeyValuePair>();
            pairList.Add(thicknessAverageAbs);
            pairList.Add(thicknessAverageRel);
            pairList.Add(thicknessStdAbs);
            pairList.Add(thicknessStdRel);
            pairList.Add(ttvStdAbs);
            pairList.Add(sawmarkStdAbs);
            pairList.Add(resistivityStdAbs);
            pairList.Add(resistivityStdRel);



            readerWriter.WriteToFile(pairList, Path.Combine(specificationPath, $"DataNamePairs{_fileExtension}"));
        }

    }
}
