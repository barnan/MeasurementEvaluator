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
        IToolMeasurementData Read();
        //List<string> CheckFileExtension();

        string ToolName { get; set; }
        List<string> InputFileList { get; set; }
        List<string[]> ExtensionList { get; set; }
        //List<IMeasDataFile> MeasDataFileList { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ToolMeasDataReader : IMeasDataFileReader
    {
        public string ToolName { get; set; }
        public List<string> InputFileList { get; set; }
        public List<string[]> ExtensionList { get; set; }
        private List<IMeasDataFile> MeasDataFileList { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="toolname"></param>
        /// <param name="extensionList"></param>
        public ToolMeasDataReader(List<string> inputs, string toolname, List<string[]> extensionList)
        {
            if (inputs == null || inputs.Count == 0 || toolname == null || extensionList== null || extensionList.Count == 0)
                return;

            InputFileList = inputs;
            ToolName = toolname;
            ExtensionList = extensionList;


            if (InputFileList != null && InputFileList.Count > 0)
            {
                List<string> finalFileList = CheckFileExtension();

                if (finalFileList != null)
                {
                    MeasDataFileList = new List<IMeasDataFile>(finalFileList.Count);

                    foreach (var item in finalFileList)
                    {
                        if (Path.GetExtension(item) == ".xml")
                        {
                            MeasDataFileList.Add(new XmlReader(item, ToolName));         // xml-reader
                        }
                        else
                        {
                            char separator = Convert.ToChar(ExtensionList.Find(p => p[0] == Path.GetExtension(item))[1]);

                            MeasDataFileList.Add(new TabularTextReader(item, ToolName, separator));    // tabular-text-reader
                        }
                    }
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IToolMeasurementData Read()
        {
            IToolMeasurementData finalResu = new ToolMeasurementData();

            if (MeasDataFileList != null && MeasDataFileList.Count > 0)
            {
                List<IToolMeasurementData> resuList = new List<IToolMeasurementData>(MeasDataFileList.Count);

                foreach (var measdatafiles in MeasDataFileList)
                {
                    resuList.Add(measdatafiles.ReadFile());
                }

                //Join data from different files into 1 ToolMeasurementData:
                finalResu = JoinToolMeasurementData(resuList);
            }

            return finalResu;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        private IToolMeasurementData JoinToolMeasurementData(List<IToolMeasurementData> inputList)
        {
            IToolMeasurementData summadata = new ToolMeasurementData(inputList[0].Name);

            if (inputList == null || inputList.Count == 0)
                return summadata;

            foreach (var toolmeasdata in inputList)
            {
                foreach (var measdata in toolmeasdata.Results)
                {
                    int index = summadata.Results.FindIndex(p => p.Name == measdata.Name);

                    if (index != -1)
                    {
                        summadata[index].MeasData.AddRange(measdata.MeasData);
                    }
                    else    // new element must be created
                    {
                        summadata.Add(new QuantityMeasurementData { Name=measdata.Name, MeasData=measdata.MeasData });
                    }
                }
            }

            return summadata;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<string> CheckFileExtension()
        {
            List<string> descriptionList = ExtensionList.Select(a => a[0]).ToList();

            // get list of appropriate files:
            List<string> finalFileList = InputFileList.Select((item, index) => new { item, index }).Where(p => descriptionList.Contains(Path.GetExtension(p.item))).Select(p => p.item).ToList();

            return finalFileList;
        }

    }


}
