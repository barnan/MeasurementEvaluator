using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    public enum TTRMeasDataFilextensions
    {
        [Description(".csv")]
        csv,
        [Description(".txt")]
        txt
    }


    class TTRMeasDataFileReader: MeasDataFileReaderBase
    {    


        public TTRMeasDataFileReader(List<string> inputs)
            :base(inputs, "TTR")
        {

            if (InputFileList != null && InputFileList.Count > 0)
            {

                List<string> enumDescriptions = new List<string>();
                foreach (var enumItem in Enum.GetValues(typeof(TTRMeasDataFilextensions)))
                {
                    FieldInfo fi = enumItem.GetType().GetField(enumItem.ToString());
                    DescriptionAttribute[] attrib = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    enumDescriptions.Add(attrib[0].ToString());
                }
                
                List<string> finalFileList = CheckFileExtension(InputFileList, enumDescriptions);

                if (finalFileList != null)
                {
                    foreach (var item in finalFileList)
                    {

                    }
                }

            }
        }




    }

}
