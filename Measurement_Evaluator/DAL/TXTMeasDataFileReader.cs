using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    class TXTMeasDataFileReader
    {
        string _file;
        string _toolName;

        public TXTMeasDataFileReader(string fileName, string toolname)
        {
            _file = fileName;
            _toolName = toolname;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="measInputFile"></param>
        /// <param name="ResListDouble"></param>
        /// <returns></returns>
        public IToolMeasurementData ReadMeasurementData_TXT_File()
        {
            string line;
            int linecounter;
            Regex[] regExp = new Regex[5];
            regExp[0] = new Regex(@"\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}");
            regExp[1] = new Regex(@"\d{4}.\d{2}.\d{2} \d{2}:\d{2}:\d{2}");
            regExp[2] = new Regex(@"\d{4}.\d{2}.\d{2} \d{2}:\d{2}:\d{2}");
            regExp[3] = new Regex(@"\d{2}:\d{2}:\d{4} \d{2}:\d{2}:\d{2}");
            regExp[4] = new Regex(@"\d{2}.\d{2}.\d{2}");


            return new ToolMeasurementData();


            //try
            //{
            //    linecounter = 1;

            //    using (StreamReader sr = new StreamReader(measInputFile))
            //    {
            //        while ((line = sr.ReadLine()) != null)
            //        {
            //            if (linecounter == 1)
            //            {
            //                string[] tokens = line.Split();
            //            }
            //            else
            //            {
            //                Match mat = null;
            //                foreach (Regex rege in regExp)
            //                {
            //                    mat = rege.Match(line);
            //                    if (mat.Length != 0)
            //                        break;
            //                }

            //                measDate = DateTime.Parse(mat.ToString());

            //                int counter3 = 0;
            //                string[] tokens = line.Split();
            //                List<double> elements = new List<double>();

            //                for (int i = 0; i < tokens.Length; i++)
            //                {
            //                    if (!tokens[i].Equals(""))
            //                    {
            //                        if (counter3 > 1)
            //                        {
            //                            try
            //                            {
            //                                elements.Add(Convert.ToDouble(tokens[i], System.Globalization.CultureInfo.InvariantCulture));
            //                            }
            //                            catch (Exception e)
            //                            {
            //                                elements.Add(0.0);
            //                            }
            //                        }
            //                        counter3++;
            //                    }
            //                }
            //                ResListDouble.Add(elements);
            //            }
            //            linecounter++;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Problem during the text file reading(" + Path.GetFileName(measInputFile) + ")." + ex.Message);
            //}

            //return true;
        }




    }
}
