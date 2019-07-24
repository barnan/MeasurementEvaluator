using DataStructures.MeasuredData;
using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Interfaces.BaseClasses;

namespace DataAcquisitions.HDDTabularMeasurementFileReaderWriter
{
    internal class HDDTabularMeasurementFileReaderWriter : IHDDFileReaderWriter
    {

        private readonly Parameters _parameters;
        private readonly object _lockObject = new object();


        internal HDDTabularMeasurementFileReaderWriter(Parameters parameter)
        {
            _parameters = parameter;
        }

        public bool WriteToFile(object obj, string fileNameAndPath)
        {
            throw new NotImplementedException();
        }


        public object ReadFromFile(string fileNameAndPath, Type type, ToolNames toolName = null)
        {
            lock (_lockObject)
            {
                try
                {
                    if (!CanRead(fileNameAndPath))
                    {
                        _parameters.Logger.MethodError($"File is not readable: {fileNameAndPath}");
                        return null;
                    }

                    return ReadTabularDataFile(fileNameAndPath, toolName);
                }
                catch (Exception ex)
                {
                    _parameters.Logger.MethodError($"Exception occured: {ex}");
                    return null;
                }
            }
        }

        private IToolMeasurementData ReadTabularDataFile(string fileNameAndPath, ToolNames toolName)
        {
            List<IMeasurementSerie> results = new List<IMeasurementSerie>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (StreamReader reader = new StreamReader(File.OpenRead(fileNameAndPath)))
            {
                bool firstLine = true;
                List<string> headers = new List<string>();
                List<List<IMeasurementPoint>> uniqueResult = new List<List<IMeasurementPoint>>();

                while (!reader.EndOfStream)
                {
                    if (sw.ElapsedMilliseconds > _parameters.FileReadTimeout)
                    {
                        break;
                    }

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
                            headers.Add(string.IsNullOrEmpty(str) ? "Empty_" + emptycounter : str);
                            uniqueResult.Add(new List<IMeasurementPoint>());
                        }

                        firstLine = false;
                    }
                    else
                    {
                        for (int i = 0; i < elements.Length; i++)
                        {
                            bool valid = double.TryParse(elements[i], out double szam);

                            if (i >= uniqueResult.Count)
                            {
                                _parameters.Logger.LogError($"Index ({i}) is higher than the length of the list ({uniqueResult.Count}). Element can not be stored.");
                                continue;
                            }

                            uniqueResult[i].Add(new MeasurementPoint(szam, valid));
                        }

                        if (elements.Length < headers.Count)
                        {
                            for (int i = elements.Length; i < headers.Count; i++)
                            {
                                uniqueResult[i].Add(new MeasurementPoint(0, false));
                                _parameters.Logger.LogTrace($"Zero element added in the {i}th element");
                            }
                        }
                    }
                }

                for (int i = 0; i < headers.Count; i++)
                {
                    results.Add(new MeasurementSerie(headers[i], uniqueResult[i]));
                }
            }

            return new ToolMeasurementData { ToolName = toolName, Results = results, Name = fileNameAndPath };
        }

        public bool CanRead(string fileNameAndPath)
        {
            if (string.IsNullOrEmpty(fileNameAndPath))
            {
                throw new ArgumentNullException($"{fileNameAndPath} can not read.");
            }

            using (FileStream fstream = new FileStream(fileNameAndPath, FileMode.Open, FileAccess.Read))
            {
                if (fstream.CanRead)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
