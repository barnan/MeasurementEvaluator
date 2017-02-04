using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Measurement_Evaluator.DAL
{
    //public enum TTRMeasDataFilextensions
    //{
    //    [Description(".csv")]
    //    [TypeText(";")]
    //    csv,
    //    [Description(".txt")]
    //    [TypeText("\t")]
    //    txt
    //}

    //public class TypeTextAttribute : Attribute
    //{
    //    public string TypeText;
    //    public TypeTextAttribute(string typeText)
    //    {
    //        TypeText = typeText;
    //    }
    //}


    class TTRMeasDataFileReader: ToolReaderBase
    {
        const string _toolName = "TTR";

        public TTRMeasDataFileReader(List<string> inputs)
            :base(inputs, _toolName, new List<string[]> { new string[2] { ".csv", ";" }, new string[2] { ".xml", "" } })
        {

            if (InputFileList != null && InputFileList.Count > 0)
            {

                //List<string> enumDescriptions = new List<string>();
                //foreach (var enumItem in Enum.GetValues(typeof(TTRMeasDataFilextensions)))
                //{
                //    FieldInfo fi = enumItem.GetType().GetField(enumItem.ToString());
                //    DescriptionAttribute[] attrib = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                //    enumDescriptions.Add(attrib[0].ToString());
                //}
                


                List<string> finalFileList = CheckFileExtension();



                if (finalFileList != null)
                {
                    MeasDataFileList = new List<IMeasDataFile>(finalFileList.Count);

                    foreach (var item in finalFileList)
                    {
                        if (Path.GetExtension(item) == ".xml" )
                        {
                            MeasDataFileList.Add(new XmlReader(item, _toolName));         // xml-reader
                        }
                        else
                        {
                            char separator = Convert.ToChar(ExtensionList.Find(p => p[0]==Path.GetExtension(item))[0]); 
                            //if (!GetSeparatorForExtension(Path.GetExtension(item), out separator))
                            //{
                            //    separator = ';';
                            //}

                            MeasDataFileList.Add(new TabularTextReader(item, _toolName, separator));    // tabular-text-reader
                        }
                    }

                    foreach (var item in MeasDataFileList)
                    {
                        item.ReadFile();
                    }

                }

            }
        }

        //private bool GetSeparatorForExtension(string fileExtension, out char resu)
        //{
            //resu = (char)0;
            //foreach (var enumItem in Enum.GetValues(typeof(TTRMeasDataFilextensions)))
            //{

            //    FieldInfo fi = enumItem.GetType().GetField(enumItem.ToString());
            //    DescriptionAttribute[] dia = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //    TypeTextAttribute[] tta = (TypeTextAttribute[])fi.GetCustomAttributes(typeof(TypeTextAttribute), false);

            //    if (string.Compare(dia[0].ToString(), fileExtension) == 0)
            //    {
            //        char c = Convert.ToChar(tta[0].ToString());
            //        return true;
            //    }
            //}
            //return false;
        //}
    }

}
