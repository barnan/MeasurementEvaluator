using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using System.IO;

namespace DataAcquisition.DAL
{

    public abstract class MeasurementDataFileBase : IMeasDataFile
    {
        public bool CanRead(string fileNameAndPath)
        {
            if (!string.IsNullOrEmpty(fileNameAndPath))
            {
                using (FileStream fstream = new FileStream(fileNameAndPath, FileMode.Open, FileAccess.Read))
                {
                    if (fstream.CanRead)
                        return true;
                }
            }
            return false;
        }


        protected bool CheckFilePath(string fileNameAndPath)
        {
            return File.Exists(fileNameAndPath);
        }


        public abstract IToolMeasurementData ReadFile(string fileNameAndPath, string toolName);

    }



    ///// <summary>
    ///// 
    ///// </summary>
    //class TabularTextMeasDataFile : MeasDataFile
    //{
    //    public char Separator { get; set; }
    //    public string ToolName { get; set; }

    //    public TabularTextMeasDataFile(string filename, string toolName, char separator)
    //        : base(filename)
    //    {
    //        Separator = separator;
    //        ToolName = toolName;
    //    }


    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public override IToolMeasurementData ReadFile()
    //    {
    //        if (CanRead())
    //        {
    //            TabularTextReader ttr = new TabularTextReader(FileName, ToolName, Separator);
    //            return ttr.ReadFile();
    //        }
    //        else
    //            return null;
    //    }

    //}



    ///// <summary>
    ///// 
    ///// </summary>
    //class XmlMeasDataFile : MeasDataFileBase
    //{
    //    public XmlMeasDataFile(string filename, string toolName)
    //        : base(filename, toolName)
    //    {
    //    }


    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public override IToolMeasurementData ReadFile()
    //    {
    //        throw new NotImplementedException();
    //    }

    //}



    ///// <summary>
    ///// 
    ///// </summary>
    //class TxtMeasDataFile : MeasDataFile
    //{
    //    public TxtMeasDataFile(string filename)
    //        : base(filename)
    //    { }


    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public override IToolMeasurementData ReadFile()
    //    {
    //        if (CanRead())
    //        {
    //            TXTMeasDataFileReader txtReader = new TabularTextReader(FileName, "TTR", '\t');
    //            return txtReader.ReadMeasurementData_TXT_File();
    //        }
    //        else
    //            return null;

    //    }

    //}


}
