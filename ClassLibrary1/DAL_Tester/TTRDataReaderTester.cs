using System;
using System.Collections.Generic;
using System.IO;
using Measurement_Evaluator.DAL;
using Measurement_Evaluator.Interfaces;
using NUnit.Framework;

namespace ClassLibrary1.DAL_Tester
{
    [TestFixture]
    class TTRDataReaderTester
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

            folder1 = dirName.ToString() + @"\DAL_Tester\TTR_InputData";
        }


        #region only txt extension
        /// <summary>
        /// 
        /// </summary>
        [Test, Category("TTRMeasurementDataReading")]
        public void TestReadingOfTTRMeasurementData1()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(    new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt"}, 
                                                                    "TTR", 
                                                                    new List<string[]> { new string[] {".txt", "\t" }                                                                                        }
                                                               );

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 11;
            int expectedRowCount = 10;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            Assert.AreEqual(expectedRowCount, data.Results[0].MeasData.Count);
        }

        #endregion
        
        #region only csv extension
        /// <summary>
        /// 
        /// </summary>
        [Test, Category("TTRMeasurementDataReading")]
        public void TestReadingOfTTRMeasurementData2()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" },
                                                                    "TTR",
                                                                    new List<string[]> { new string[] {".csv", ";" }
                                                               });

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 11;
            int expectedRowCount = 10;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            Assert.AreEqual(expectedRowCount, data.Results[0].MeasData.Count);
        }

        #endregion

        #region txt and csv

        /// <summary>
        /// 
        /// </summary>
        [Test, Category("TTRMeasurementDataReading")]
        public void TestReadingOfTTRMeasurementData3()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" },
                                                                    "TTR",
                                                                    new List<string[]> { new string[] {".csv", ";" },
                                                                                         new string[] {".txt", "\t" }
                                                                                 });

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 13;
            int expectedRowCount1 = 20;
            int expectedRowCount2 = 10;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            Assert.AreEqual(expectedRowCount1, data.Results[0].MeasData.Count);
            Assert.AreEqual(expectedRowCount2, data.Results[expectedColumnCount-1].MeasData.Count);
        }

        #endregion
        
        #region null extension list

        /// <summary>
        /// checks the response in case of NULL extension list
        /// </summary>
        [Test, Category("TTRMeasurementDataReading")]
        public void TestReadingOfTTRMeasurementData5()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" },
                                                                    "TTR",
                                                                    null);

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 0;
            int expectedRowCount = 0;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            try
            {
                Assert.AreEqual(expectedRowCount, data.Results[0]?.MeasData.Count);
            }
            catch(ArgumentOutOfRangeException)
            {
                Assert.Pass();

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        #endregion

        #region empty extension list

        /// <summary>
        /// checks the response in case of EMPTY extension list
        /// </summary>
        [Test, Category("TTRMeasurementDataReading")]
        public void TestReadingOfTTRMeasurementData6()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" },
                                                                    "TTR",
                                                                    new List<string[]> ());

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 0;
            int expectedRowCount = 0;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            try
            {
                Assert.AreEqual(expectedRowCount, data.Results[0].MeasData.Count);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.Pass();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        #endregion

        #region empty filelist

        /// <summary>
        /// checks the response in case of EMPTY file list
        /// </summary>
        [Test, Category("TTRMeasurementDataReading")]
        public void TestReadingOfTTRMeasurementData7()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> (),
                                                                    "TTR",
                                                                   new List<string[]> { new string[] { ".csv", ";" } });

            IToolMeasurementData data = reader.Read();

            int expectedColumnCount = 0;
            int expectedRowCount = 0;

            Assert.AreEqual(expectedColumnCount, data.Results.Count);
            try
            {
                Assert.AreEqual(expectedRowCount, data.Results[0].MeasData.Count);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.Pass();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        #endregion

        #region empty columns and/or empty headers

        public void TestReadingOfTTRMeasurementData8()
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(new List<string> { folder1 + @"\\TTR_TestMeasData_3.csv" },
                                                                    "TTR",
                                                                    new List<string[]> { new string[] { ".csv", ";" } }
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
