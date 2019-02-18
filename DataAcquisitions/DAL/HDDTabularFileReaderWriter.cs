﻿using DataStructures.MeasuredData;
using Interfaces;
using Interfaces.MeasuredData;
using Miscellaneous;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAcquisitions.DAL
{
    public class HDDTabularFileReaderWriter : HDDFileReaderWriterBase
    {

        private readonly TabularTextReaderParameters _parameters;


        public HDDTabularFileReaderWriter(TabularTextReaderParameters parameter)
        {
            _parameters = parameter;
        }



        public override bool WriteToFile<T>(T obj, string fileNameAndPath)
        {
            throw new NotImplementedException();
        }



        public override T ReadFromFile<T>(string fileNameAndPath, ToolNames toolName = null)
        {
            lock (_lockObject)
            {
                T data;

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

                    if (typeof(T) != typeof(IToolMeasurementData))
                    {
                        _parameters.Logger.MethodError($"The {nameof(HDDTabularFileReaderWriter)} can read only tabular data, {typeof(T)} can not be handled.");
                        return default(T);
                    }

                    data = (T)ReadTabularDataFile(fileNameAndPath, toolName);
                }
                catch (Exception ex)
                {
                    _parameters.Logger.MethodError($"Exception occured: {ex}");
                    return default(T);
                }

                return data;
            }
        }



        private IToolMeasurementData ReadTabularDataFile(string fileNameAndPath, ToolNames toolName)
        {
            List<IMeasurementSerie> results = new List<IMeasurementSerie>();

            using (StreamReader reader = new StreamReader(File.OpenRead(fileNameAndPath)))
            {
                bool firstLine = true;
                List<string> headers = new List<string>();
                List<List<IMeasurementPoint>> uniqueResult = new List<List<IMeasurementPoint>>();

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
                            headers.Add(string.IsNullOrEmpty(str) ? "Empty_" + emptycounter : str);
                            uniqueResult.Add(new List<IMeasurementPoint>());
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

            ToolMeasurementData toolMeasData = new ToolMeasurementData { ToolName = toolName, Results = results, Name = fileNameAndPath };
            return toolMeasData;
        }
    }



    public class TabularTextReaderParameters
    {
        public char Separator { get; set; }
        public ILogger Logger { get; set; }
    }



}