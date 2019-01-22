using DataStructures.MeasuredData;
using Interfaces.MeasuredData;
using Miscellaneous;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAcquisition.DAL
{
    public class HDDTabularTextReader : MeasurementDataFileBase
    {
        private readonly TabularTextReaderParameters _parameters;


        public HDDTabularTextReader(TabularTextReaderParameters parameter)
        {
            _parameters = parameter;
        }


        public override bool WriteFile<T>(T obj, string fileNameAndPath)
        {
            throw new NotImplementedException();
        }


        public override T ReadFile<T>(string fileNameAndPath, string toolName = null)
        {
            T toolMeasData;

            try
            {
                if (!CheckFilePath(fileNameAndPath))
                {
                    _parameters.Logger.MethodError($"File does not exists: {fileNameAndPath}");
                    return default(T);
                }

                if (!CanRead(fileNameAndPath))
                {
                    _parameters.Logger.MethodError($"File is not readable: {fileNameAndPath}");
                    return default(T);
                }

                toolMeasData = (T)ReadTabularDataFile(fileNameAndPath, toolName);
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Exception occured: {ex}");
                return default(T);
            }

            return toolMeasData;
        }


        private IToolMeasurementData ReadTabularDataFile(string fileNameAndPath, string toolName)
        {
            ToolMeasurementData toolMeasData = new ToolMeasurementData { ToolName = toolName, Results = new List<IMeasurementSerie>(), FullNameOnHDD = fileNameAndPath };

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
                            bool valid;
                            try
                            {
                                szam = Convert.ToDouble(elements[i], System.Globalization.CultureInfo.InvariantCulture);
                                valid = true;
                            }
                            catch (FormatException ex)
                            {
                                szam = 0.0;
                                valid = false;
                            }

                            toolMeasData.Results[i].MeasData.Add(new UniqueMeasurementResult<double>(szam, valid));
                        }
                    }
                }
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
