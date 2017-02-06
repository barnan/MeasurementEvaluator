using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.DAL;
using Measurement_Evaluator.Interfaces;
using NUnit.Framework;

namespace ClassLibrary1.DAL_Tester
{
    [TestFixture]
    class SHPDataReaderTester
    {
        /// <summary>
        /// contains the 
        /// </summary>
        string folder1;

        /// <summary>
        /// 
        /// </summary>
        [OneTimeSetUp]
        public void SetInputDir()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            DirectoryInfo dirName = Directory.GetParent(Directory.GetParent(path.Substring(6, path.Length - 6)).ToString());

            folder1 = dirName.ToString() + @"\DAL_Tester\SHP_InputData";
        }


        #region only csv data file

        [Test, Category("SHPMeasurementDataReading")]
        public void TestReadingOfSHPMeasurementData1()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" },
                                                                       "SHP",
                                                                       new List<string[]> { new string[] { ".csv", ";" } }
                                                                  );

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 11;
            int expectedRowCount = 10;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            Assert.AreEqual(expectedRowCount, data.Results[0].MeasData.Count);
        }

        #endregion


        #region only xml data files

        [Test, Category("SHPMeasurementDataReading")]
        public void TestReadingOfSHPMeasurementData2()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> { folder1 + @"\\SimulatedWaferID1.xml" },
                                                                       "SHP",
                                                                       new List<string[]> { new string[] { ".xml", "" } }
                                                                  );

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 11;
            int expectedRowCount = 10;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            Assert.AreEqual(expectedRowCount, data.Results[0].MeasData.Count);
        }

        #endregion



    }
}
