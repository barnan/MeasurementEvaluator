using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Measurement_Evaluator.Interfaces;
using System.Xml.Schema;

namespace Measurement_Evaluator.BLL
{
    [Serializable]
    public class ToolSpecification :IToolSpecification
    {
        public string ToolName { set; get; }

        public List<QuantitySpecification> ToolspecificationList { get; set; }          // -> szerializálásra??
        [XmlIgnore]
        public List<IQuantitySpecification> SpecificationList
        {
            get
            {
                return ToolspecificationList?.Select(c => (IQuantitySpecification)c).ToList();
            }
            set
            {
                if (value?.Count > 0)
                {
                    ToolspecificationList.Clear();
                    foreach (var item in value)
                        ToolspecificationList.Add((QuantitySpecification)item);
                }
                else
                {
                    ToolspecificationList.Clear();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toolname"></param>
        /// <param name="spec"></param>
        public ToolSpecification(string toolname, List<IQuantitySpecification> spec)
        {
            ToolspecificationList = new List<QuantitySpecification>();

            ToolName = toolname;
            SpecificationList = spec;
        }

        /// <summary>
        /// 
        /// </summary>
        public ToolSpecification()
        {
            ToolName = string.Empty;
            ToolspecificationList = new List<QuantitySpecification>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        public static void Save(string FileName, ToolSpecification toolspec)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(typeof(ToolSpecification));
                serializer.Serialize(writer, toolspec);
                writer.Flush();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        public static ToolSpecification Read(string FileName)
        {
            ToolSpecification ts;

            using (var reader = new System.IO.StreamReader(FileName))
            {
                var serializer = new XmlSerializer(typeof(ToolSpecification));
                ts = (ToolSpecification)serializer.Deserialize(reader);
            }
            return ts;
        }


    }

    
}
