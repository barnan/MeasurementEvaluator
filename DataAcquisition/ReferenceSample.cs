using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    [Serializable]
    public class ReferenceSample :IReferenceSample
    {
        public string SampleID { get; set; }
        public Orientation SampleOrientation { get; set; }

        public List<ReferenceValue> ListOfRefVals = new List<ReferenceValue>();
        [XmlIgnore]
        public List<IReferenceValue> ListOfReferenceValues
        {
            get
            {
                return ListOfRefVals.Select(c => (IReferenceValue)c).ToList();
            }
            set
            {
                if (value?.Count > 0)
                {
                    ListOfRefVals.Clear();

                    foreach (var item in value)
                        ListOfRefVals.Add((ReferenceValue)item);
                }
                else
                {
                    ListOfRefVals.Clear();
                }
            }
        }

        public ReferenceSample()
        {
        }

        public ReferenceSample(string name, List<IReferenceValue> refv, Orientation orientation)
        {
            SampleID = name;
            ListOfReferenceValues = refv;
            SampleOrientation = orientation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        public static void Save(string FileName, ReferenceSample refsamp)
        {
            try
            {
                var writer = new System.IO.StreamWriter(FileName);
                XmlSerializer serializer = new XmlSerializer(typeof(ReferenceSample));
                serializer.Serialize(writer, refsamp);
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            { }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        public static ReferenceSample Read(string FileName)
        {
            try
            {
                ReferenceSample rs;

                System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
                XmlSerializer serializer = new XmlSerializer(typeof(ReferenceSample));
                rs = (ReferenceSample)serializer.Deserialize(reader);
                reader.Close();

                return rs;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
