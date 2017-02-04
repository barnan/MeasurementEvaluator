using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Measurement_Evaluator.DAL
{
    public enum SHPMeasDataFilextensions
    {
        [Description(".csv")]
        csv,
        [Description(".xml")]
        xml
    }


    class SHPToolReader : ToolReaderBase
    {
        const string _toolName = "SHP";


        public SHPToolReader(List<string> inputs) 
            : base(inputs, _toolName, new List<string[]> { new string[2] { ".csv", ";" }, new string[2] { ".xml", "" } })
        {

            if (InputFileList != null && InputFileList.Count > 0)
            {

                List<string> enumDescriptions = new List<string>();
                foreach (var enumItem in Enum.GetValues(typeof(SHPMeasDataFilextensions)))
                {
                    FieldInfo fi = enumItem.GetType().GetField(enumItem.ToString());
                    DescriptionAttribute[] attrib = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    enumDescriptions.Add(attrib[0].ToString());
                }

                List<string> finalFileList = CheckFileExtension();

                if (finalFileList != null)
                {
                    MeasDataFileList = new List<IMeasDataFile>(finalFileList.Count);

                    foreach (var item in finalFileList)
                    {
                        if (Path.GetExtension(item) == ".xml")
                        {
                            MeasDataFileList.Add(new XmlReader(item, _toolName));         // xml-reader
                        }
                        else
                        {
                            MeasDataFileList.Add(new TabularTextReader(item, _toolName, ';'));    // tabular-text-reader
                        }
                    }

                    foreach (var item in MeasDataFileList)
                    {
                        item.ReadFile();
                    }

                }

            }



        }



        
    }
}
