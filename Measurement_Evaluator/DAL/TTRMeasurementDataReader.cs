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


    /// <summary>
    /// 
    /// </summary>
    interface ITTRDataFileReader : IDataFileReader
    {
    }


    /// <summary>
    /// 
    /// </summary>
    class CsvDataFileReader : ITTRDataFileReader
    {

        public IToolMeasurementData Read(string fileName)
        {
            CSVReader csvr = new CSVReader(fileName, "TTR", ";");

            return csvr.ReadMeasData_CSV_File();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class TxtDataFileReader : ITTRDataFileReader
    {
        public IToolMeasurementData Read(string fileName)
        {
            TXTReader txtReader = new TXTReader(fileName, "TTR");

            return txtReader.ReadMeasurementData_TXT_File();

        }
    }


    class TTRMeasurementDataReader : IMeasurementDataReader
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"> List of input data files</param>
        /// <returns></returns>
        public IToolMeasurementData ReadMeasurementData(List<string> inputs)
        {
            List<string> finalFileList = CheckFileExtension(inputs);

            if (finalFileList == null)
                return null;

            List<ITTRDataFileReader> ttrDataFileList = new List<ITTRDataFileReader>();
            
            foreach (var item in finalFileList)
            {
                if (Path.GetExtension(item) == ".csv")
                    ttrDataFileList.Add(new CsvDataFileReader());
                else if (Path.GetExtension(item) == ".ttr")
                    ttrDataFileList.Add(new CsvDataFileReader());
            }

            return new ToolMeasurementData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs">List of input data files</param>
        /// <returns></returns>
        private List<string> CheckFileExtension(List<string> inputs)
        {
            if (inputs?.Count == 0)
                return null;

            List<string> enumDescriptions = new List<string>();
            foreach (var enumItem in Enum.GetValues(typeof(TTRMeasDataFilextensions)))
            {

                // get list of enum descriptions:
                FieldInfo fi = enumItem.GetType().GetField(enumItem.ToString());
                DescriptionAttribute[] attrib = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                enumDescriptions.Add(attrib[0].ToString());
            }

            // get list of appropriate files:
            List<string> finalFileList = inputs.Select((item, index) => new { item, index }).Where(p => enumDescriptions.Contains(Path.GetExtension(p.item))).Select(p => p.item).ToList();

            return finalFileList;
        }
    }


}
