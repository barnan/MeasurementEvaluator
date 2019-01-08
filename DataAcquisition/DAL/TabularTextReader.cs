using DataStructures.MeasuredData;
using Interfaces.MeasuredData;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAcquisition.DAL
{
    public class TabularTextReader : MeasurementDataFileBase
    {
        private char _separator;

        public TabularTextReader(char separator = ';')
        {
            _separator = separator;
        }


        /// <summary>
        /// Reads the file content -> returns with the full IToolMeasurement result
        /// </summary>
        /// <returns></returns>
        public override IToolMeasurementData ReadFile(string fileNameAndPath, string toolName)
        {
            IToolMeasurementData toolMeasData = null;

            try
            {
                if (CheckFilePath(fileNameAndPath) && CanRead(fileNameAndPath))
                {
                    toolMeasData = new ToolMeasurementData(toolName, new List<IMeasurementSerie>());

                    using (StreamReader reader = new StreamReader(File.OpenRead(fileNameAndPath)))
                    {
                        bool firstLine = true;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            if (line == null)
                            {
                                continue;
                            }

                            string[] elements = line.Split(_separator);

                            if (firstLine)
                            {
                                int emptycounter = 0;

                                foreach (string str in elements)
                                {
                                    if (string.IsNullOrEmpty(str))
                                    {
                                        toolMeasData.Results.Add(new MeasurementSerie("Empty_" + emptycounter, new List<IUniqueMeasurementResult>()));
                                    }
                                    else
                                    {
                                        toolMeasData.Results.Add(new MeasurementSerie(str, new List<IUniqueMeasurementResult>()));
                                    }
                                }

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

                                    toolMeasData.Results[i].MeasData.Add(new UniqueMeasurementResult<double>(szam));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: error in file read
                return toolMeasData;
            }

            return toolMeasData;
        }

    }
}
