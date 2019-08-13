using DataStructures.MeasuredData;
using Interfaces.BaseClasses;
using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
                    List<string> headers = ReadTabularHeaders(fileNameAndPath);
                    return ReadTabularDataFile(fileNameAndPath, toolName, headers);
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogMethodError($"Exception occured: {ex}");
                    return null;
                }
            }
        }

        private List<string> ReadTabularHeaders(string fileNameAndPath)
        {
            List<string> headers = new List<string>();

            using (StreamReader reader = new StreamReader(File.OpenRead(fileNameAndPath)))
            {
                string line = reader.ReadLine();
                string[] elements = line.Split(_parameters.Separator);

                int emptycounter = 0;
                foreach (string str in elements)
                {
                    headers.Add(string.IsNullOrEmpty(str) ? "Empty_" + emptycounter++ : str);
                }
            }
            return headers;
        }

        private IToolMeasurementData ReadTabularDataFile(string fileNameAndPath, ToolNames toolName, List<string> header)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<IMeasurementSerie> results = new List<IMeasurementSerie>();
            List<List<INumericMeasurementPoint>> uniqueResult = new List<List<INumericMeasurementPoint>>();

            for (int i = 0; i < header.Count; i++)
            {
                uniqueResult.Add(new List<INumericMeasurementPoint>());
            }

            using (StreamReader reader = new StreamReader(File.OpenRead(fileNameAndPath)))
            {
                while (!reader.EndOfStream && sw.ElapsedMilliseconds > _parameters.FileReadTimeout)
                {
                    string line = reader.ReadLine();
                    string[] elements = line.Split(_parameters.Separator);

                    //if (elements.Length != header.Count)
                    //{
                    //    throw new FileLoadException("File contains more or less data rows than the first line (header)");
                    //}

                    for (int i = 0; i < elements.Length; i++)
                    {
                        bool valid = double.TryParse(elements[i], out double szam);
                        uniqueResult[i].Add(new NumericMeasurementPoint(szam, valid));
                    }
                }

                for (int i = 0; i < header.Count; i++)
                {
                    results.Add(new MeasurementSerie(header[i], uniqueResult[i]));
                }
            }

            return new ToolMeasurementData { ToolName = toolName, Results = results, Name = fileNameAndPath };
        }

    }
}
