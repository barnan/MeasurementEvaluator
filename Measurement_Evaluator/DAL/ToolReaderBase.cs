using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{

    /// <summary>
    /// 
    /// </summary>
    public interface IMeasDataFileReader
    {
        IToolMeasurementData Read(string fileName);
        List<string> CheckFileExtension();

        string ToolName { get; set; }
        List<string> InputFileList { get; set; }
        List<string[]> ExtensionList { get; set; }
        List<IMeasDataFile> MeasDataFileList { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public abstract class ToolReaderBase : IMeasDataFileReader
    {
        public string ToolName { get; set; }
        public List<string> InputFileList { get; set; }
        public List<IMeasDataFile> MeasDataFileList { get; set; }

        public List<string[]> ExtensionList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="toolname"></param>
        public ToolReaderBase(List<string> inputs, string toolname, List<string[]> extensionList)
        {
            InputFileList = inputs;
            ToolName = toolname;

            ExtensionList = extensionList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IToolMeasurementData Read(string fileName)
        {
            List<IToolMeasurementData> resuList = new List<IToolMeasurementData>(MeasDataFileList.Count);

            foreach (var measdatafiles in MeasDataFileList)
            {
                resuList.Add(measdatafiles.ReadFile());
            }

            //Join data from different files into 1 ToolMeasurementData:
            IToolMeasurementData finalResu = JoinToolMeasurementData(resuList);

            return finalResu;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        private IToolMeasurementData JoinToolMeasurementData(List<IToolMeasurementData> inputList)
        {
            if (inputList == null || inputList.Count == 0)
                return null;

            IToolMeasurementData summadata = new ToolMeasurementData(inputList[0].Name);

            foreach (var toolmeasdata in inputList)
            {
                foreach (var measdata in toolmeasdata.Results)
                {
                    int index = summadata.Results.FindIndex(p => p.Name == measdata.Name);

                    if (index != -1)
                    {
                        summadata.Results[index].MeasData.AddRange(measdata.MeasData);
                    }
                    else    // new element must be created
                    {
                        summadata.Results.Add(new QuantityMeasurementData { Name=measdata.Name, MeasData=measdata.MeasData });
                    }
                }
            }

            return summadata;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="enumDescriptions"></param>
        /// <returns></returns>
        //public List<string> CheckFileExtension(List<string> inputs, List<string> enumDescriptions)
        public List<string> CheckFileExtension()
        {
            List<string> descriptionList = ExtensionList.Select(a => a[0]).ToList();

            // get list of appropriate files:
            List<string> finalFileList = InputFileList.Select((item, index) => new { item, index }).Where(p => descriptionList.Contains(Path.GetExtension(p.item))).Select(p => p.item).ToList();

            return finalFileList;
        }

    }


}
