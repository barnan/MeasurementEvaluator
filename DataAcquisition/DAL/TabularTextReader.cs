using Interfaces.MeasuredData;
using System;
using System.IO;

namespace DataAcquisition.DAL
{
    class TabularTextReader : MeasDataFileBase
    {
        char _separator;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="toolname"></param>
        /// <param name="sep"></param>
        public TabularTextReader(string fileName, string toolname, char sep = ';')
            : base(fileName, toolname)
        {
            _separator = sep;
        }


        /// <summary>
        /// Reads the file content -> returns with the full IToolMeasurement result
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

                            if (line == null)
                                continue;

                            string[] elements = line.Split(_separator);

                            if (firstLine)
                            {
                                int emptycounter = 0;

                                foreach (string str in elements)
                                {
                                    if (string.IsNullOrEmpty(str))
                                        toolMeasData.Add(new QuantityMeasurementData { Name = "Empty_" + emptycounter });
                                    else
                                        toolMeasData.Add(new QuantityMeasurementData { Name = str });
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
