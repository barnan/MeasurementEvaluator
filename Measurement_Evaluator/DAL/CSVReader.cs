﻿using System;
using System.Collections.Generic;
using System.IO;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    class CSVReader
    {

        string _file;
        string _separator;
        string _toolName;

        public CSVReader(string fileName, string toolname, string sep = ";" )
        {
            _file = fileName;
            _separator = sep;
            _toolName = toolname;
        }



        public IToolMeasurementData Read(string fileName)
        {
            return ReadMeasData_CSV_File();
        }


        //public bool ReadMeasData_CSV_File(string measFile, ref List<List<double>> ResListDouble, ref List<string> columnNameList, ref DateTime dateofmeas)
        public IToolMeasurementData ReadMeasData_CSV_File()
        {
            IToolMeasurementData toolMeasData = new ToolMeasurementData();
            toolMeasData.Name = _toolName;

            using (StreamReader reader = new StreamReader(File.OpenRead(_file)))
            {
                bool firstLine = true;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] elements = line.Split();

                    if (firstLine)
                    {
                        toolMeasData.Results = new List<IQuantityMeasurementData>(elements.Length);

                        for (int i = 0; i < elements.Length; i++)
                            toolMeasData.Name = elements[i]; 

                        firstLine = false;
                    }
                    else
                    {
                        for (int i = 0; i < elements.Length; i++)
                            toolMeasData.Results[i].MeasData.Add(Convert.ToDouble(elements[i], System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
                
            }

            return toolMeasData;
        }



    }
}
