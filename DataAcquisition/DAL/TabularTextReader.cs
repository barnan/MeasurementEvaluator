using DataStructures.MeasuredData;
using Interfaces.MeasuredData;
using Miscellaneous;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAcquisition.DAL
{
    public class TabularTextReader : MeasurementDataFileBase
    {
        private readonly TabularTextReaderParameters _parameters;


        public TabularTextReader(TabularTextReaderParameters parameter)
        {
            _parameters = parameter;
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
                if (!CheckFilePath(fileNameAndPath))
                {
                    _parameters.Logger.MethodError($"File does not exists: {fileNameAndPath}");
                    return null;
                }

                if (!CanRead(fileNameAndPath))
                {
                    _parameters.Logger.MethodError($"File is not readable: {fileNameAndPath}");
                    return null;
                }

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

                        string[] elements = line.Split(_parameters.Separator);

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
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Exception occured: {ex}");
                return toolMeasData;
            }

            return toolMeasData;
        }

    }

    public class TabularTextReaderParameters
    {
        public char Separator { get; set; }
        public ILogger Logger { get; set; }
    }



}
