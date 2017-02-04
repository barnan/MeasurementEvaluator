using System.Collections.Generic;
using System.IO;
using System.Linq;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{

    public abstract class MeasDataFileReaderBase : IMeasDataFileReader
    {
        public string ToolName { get; set; }
        public List<string> InputFileList { get; set; }
        public List<IMeasDataFile> MeasDataFileList { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="toolname"></param>
        public MeasDataFileReaderBase(List<string> inputs, string toolname)
        {
            InputFileList = inputs;
            ToolName = toolname;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IToolMeasurementData Read(string fileName)
        {
            List<IToolMeasurementData> resuList = new List<IToolMeasurementData>(MeasDataFileList.Count);

            foreach (var item in MeasDataFileList)
            {
                resuList.Add(item.ReadFile());
            }

            //Join data:
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
        public List<string> CheckFileExtension(List<string> inputs, List<string> enumDescriptions)
        {
            // get list of appropriate files:
            List<string> finalFileList = inputs.Select((item, index) => new { item, index }).Where(p => enumDescriptions.Contains(Path.GetExtension(p.item))).Select(p => p.item).ToList();

            return finalFileList;
        }

    }


}
