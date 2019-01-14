using Interfaces;
using System.Collections.Generic;
using IReferenceValue = Interfaces.ReferenceSample.IReferenceValue;

namespace DataAcquisition
{
    //[Serializable]
    public class ReferenceSampleOnHDD
    {
        public string SampleID { get; }


        public List<IReferenceValue> ReferenceValues { get; private set; }


        public SampleOrientation SampleOrientation { get; }



        public ReferenceSampleOnHDD(string name, List<IReferenceValue> refv, SampleOrientation orientation)
        {
            SampleID = name;
            ReferenceValues = refv;
            SampleOrientation = orientation;
        }





        //public List<ReferenceValue> ListOfRefVals = new List<ReferenceValue>();
        //private List<IReferenceValue> _referenceValues;
        //private SampleOrientation _sampleOrientation;

        //// [XmlIgnore]
        //public List<IReferenceValue> ListOfReferenceValues
        //{
        //    get
        //    {
        //        return ListOfRefVals.Select(c => (IReferenceValue)c).ToList();
        //    }
        //    set
        //    {
        //        if (value?.Count > 0)
        //        {
        //            ListOfRefVals.Clear();

        //            foreach (var item in value)
        //                ListOfRefVals.Add((ReferenceValue)item);
        //        }
        //        else
        //        {
        //            ListOfRefVals.Clear();
        //        }
        //    }
        //}

        //public ReferenceSample()
        //{
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="FileName"></param>
        //public static void Save(string FileName, ReferenceSample refsamp)
        //{
        //    try
        //    {
        //        var writer = new System.IO.StreamWriter(FileName);
        //        XmlSerializer serializer = new XmlSerializer(typeof(ReferenceSample));
        //        serializer.Serialize(writer, refsamp);
        //        writer.Flush();
        //        writer.Close();
        //    }
        //    catch (Exception ex)
        //    { }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="FileName"></param>
        //public static ReferenceSample Read(string FileName)
        //{
        //    try
        //    {
        //        ReferenceSample rs;

        //        System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
        //        XmlSerializer serializer = new XmlSerializer(typeof(ReferenceSample));
        //        rs = (ReferenceSample)serializer.Deserialize(reader);
        //        reader.Close();

        //        return rs;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}


    }
}
