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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="toolname"></param>
        /// <param name="sep"></param>
        public TabularTextReader(string fileName, string toolname, char sep = ';' )
            :base(fileName, toolname)
        {
            _separator = sep;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IToolMeasurementData ReadFile()
        {
            IToolMeasurementData toolMeasData = new ToolMeasurementData();

            try
            {
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
                                for (int i = 0; i < elements.Length; i++)
                                    toolMeasData.Add(new QuantityMeasurementData { Name = elements[i] });

                                firstLine = false;
                            }
                            else
                            {
                                for (int i = 0; i < elements.Length; i++)
                                {
                                    double szam;
                                    try
                                    {
                                        szam = Convert.ToDouble(elements[i], System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch (FormatException ex)
                                    {
                                        szam = 0.0;
                                    }


                                    toolMeasData[i].MeasData.Add(szam);
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: error in file read
            }

            return toolMeasData;
        }



    }
}
