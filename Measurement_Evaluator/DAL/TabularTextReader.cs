using System;
using System.Collections.Generic;
using System.IO;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    class TabularTextReader : MeasDataFileBase
    {
        char _separator;

        public TabularTextReader(string fileName, string toolname, char sep = ';' )
            :base(fileName, toolname)
        {
            _separator = sep;
        }


        //public bool ReadMeasData_CSV_File(string measFile, ref List<List<double>> ResListDouble, ref List<string> columnNameList, ref DateTime dateofmeas)
        public override IToolMeasurementData ReadFile()
        {
            IToolMeasurementData toolMeasData = new ToolMeasurementData();

            if (CanRead())
            {
                
                toolMeasData.Name = ToolName;

                using (StreamReader reader = new StreamReader(File.OpenRead(FileName)))
                {
                    bool firstLine = true;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        string[] elements = line.Split(_separator);

                        if (firstLine)
                        {
                            toolMeasData.Results = new List<IQuantityMeasurementData>(elements.Length);

                            for (int i = 0; i < elements.Length; i++)
                                toolMeasData.Results[i].Name = elements[i];

                            firstLine = false;
                        }
                        else
                        {
                            for (int i = 0; i < elements.Length; i++)
                                toolMeasData.Results[i].MeasData.Add(Convert.ToDouble(elements[i], System.Globalization.CultureInfo.InvariantCulture));
                        }
                    }

                }
            }

            return toolMeasData;
        }



    }
}
