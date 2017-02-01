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
    public abstract class MeasDataFile : IMeasDataFile
    {
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public MeasDataFile(string filename)
        {
            FileName = filename;
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



    /// <summary>
    /// 
    /// </summary>
    class CsvMeasDataFile : MeasDataFile
    {
        public CsvMeasDataFile(string filename)
            : base(filename)
        { }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IToolMeasurementData ReadFile()
        {
            if (CanRead())
            {
                CSVReader csvr = new CSVReader(FileName, "TTR", ";");
                return csvr.ReadMeasData_CSV_File();
            }
            else
                return null;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    class TxtMeasDataFile : MeasDataFile
    {
        public TxtMeasDataFile(string filename)
            : base(filename)
        { }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IToolMeasurementData ReadFile()
        {
            if (CanRead())
            {
                TXTMeasDataFileReader txtReader = new TXTMeasDataFileReader(FileName, "TTR");
                return txtReader.ReadMeasurementData_TXT_File();
            }
            else
                return null;

        }

    }


}
