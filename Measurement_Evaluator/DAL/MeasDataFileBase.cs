using System;
using System.Collections.Generic;
using System.IO;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMeasDataFile
    {
        string FileName { get; set; }
        IToolMeasurementData ReadFile();
        bool CanRead();
    }


    /// <summary>
    /// 
    /// </summary>
    public abstract class MeasDataFileBase : IMeasDataFile
    {
        public string FileName { get; set; }
        public string ToolName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public MeasDataFileBase(string filename, string toolname)
        {
            FileName = filename;
            ToolName = toolname;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanRead()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                if (fs.CanRead)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IToolMeasurementData ReadFile();
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
