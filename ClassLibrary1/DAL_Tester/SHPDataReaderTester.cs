using System;
using System.Collections;
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
        static string folder1 = SetupPath();


        /// <summary>
        /// get the durrent directory and navigate to the test directory
        /// </summary>
        /// <returns></returns>
        static string SetupPath()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            DirectoryInfo dirName = Directory.GetParent(Directory.GetParent(path.Substring(6, path.Length - 6)).ToString());

            return dirName.ToString() + @"\DAL_Tester\SHP_InputData";
        }


        class SHPTestFactory_csv
        {
            public static IEnumerable ColumnTestCases
            {
                get
                {
                    // only csv
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData column - only csv 1").Returns(11);
                    // only txt
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".txt", "\t" } }).SetName("SHP ReadData column - only txt 1").Returns(11);
                    // empty extension list 
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]>()).SetName("SHP ReadData column - empty extension").Returns(11);
                    // null extension list
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, null).SetName("SHP ReadData column - null extension").Returns(11);
                    // empty file list
                    yield return new TestCaseData(null, new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData column - empty file list").Returns(11);
                    // null file list
                    yield return new TestCaseData(new List<string> (), new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData column - null file list").Returns(11);
                }
            }

            public static IEnumerable RowTestCases
            {
                get
                {
                    // only csv
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData row - only csv 1").Returns(10);
                    // only txt
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".txt", "\t" } }).SetName("SHP ReadData row - only txt 1").Returns(10);
                    // empty extension list 
                    //yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]>()).SetName("SHP ReadData row - empty extension").Returns(11);
                    // null extension list
                    //yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, null).SetName("SHP ReadData row - null extension").Returns(11);
                    // empty file list
                    //yield return new TestCaseData(null, new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData row - empty file list").Returns(11);
                    // null file list
                    //yield return new TestCaseData(new List<string>(), new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData row - null file list").Returns(11);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        class SHPXmlTestFactory_csv
        {
            public static IEnumerable RowTestCases
            {
                get
                {
                    // only csv
                    yield return new TestCaseData(new List<string> {    folder1 + @"\\SimulatedWaferID1.xml",
                                                                        folder1 + @"\\SimulatedWaferID2.xml",
                                                                        folder1 + @"\\SimulatedWaferID3.xml" }, 
                                                  new List<string[]> { new string[] { ".xml", "" }, new string[] { ".csv", ";" } }).SetName("SHP ReadData row - xml and csv").Returns(3);
                    // only txt extension
                    yield return new TestCaseData(new List<string>{    folder1 + @"\\SimulatedWaferID1.xml",
                                                                        folder1 + @"\\SimulatedWaferID2.xml",
                                                                        folder1 + @"\\SimulatedWaferID3.xml" }, 
                                                  new List<string[]> { new string[] { ".xml", "\t" } }).SetName("SHP ReadData row - only txt 1").Returns(10);
                    // empty extension list 
                    yield return new TestCaseData(new List<string> {    folder1 + @"\\SimulatedWaferID1.xml",
                                                                        folder1 + @"\\SimulatedWaferID2.xml",
                                                                        folder1 + @"\\SimulatedWaferID3.xml" },
                                                  new List<string[]>()).SetName("SHP ReadData row - empty extension").Returns(11);
                    // null extension list
                    yield return new TestCaseData(new List<string> {    folder1 + @"\\SimulatedWaferID1.xml",
                                                                        folder1 + @"\\SimulatedWaferID2.xml",
                                                                        folder1 + @"\\SimulatedWaferID3.xml" }, 
                                                  null).SetName("SHP ReadData row - null extension").Returns(11);
                    // null file list
                    yield return new TestCaseData(null, new List<string[]> { new string[] { ".xml", "" } }).SetName("SHP ReadData row - empty file list").Returns(11);
                    // empty file list
                    yield return new TestCaseData(new List<string>(), new List<string[]> { new string[] { ".xml", "" } }).SetName("SHP ReadData row - null file list").Returns(11);
                }
            }
        }



        [Test, Category("SHPMeasurementDataReading"), TestCaseSource(typeof(SHPTestFactory_csv), "ColumnTestCases")]
        public int TestReadOfTTRData_ColumnCount(List<string> fileList, List<string[]> extensionList)
        {
            IMeasDataFileReader reader = new ToolMeasDataReader( fileList, "SHP", extensionList);

            IToolMeasurementData data = reader.Read();

            return data.Results.Count;
        }




        [Test, Category("SHPMeasurementDataReading"), TestCaseSource(typeof(SHPTestFactory_csv), "RowTestCases")]
        public int TestReadOfTTRData_RowCount(List<string> fileList, List<string[]> extensionList)
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(fileList, "SHP", extensionList);

            IToolMeasurementData data = reader.Read();

            return data.Results[0].MeasData.Count;
        }




        [Test, Category("SHPXmlMeasurementDataReading"), TestCaseSource(typeof(SHPXmlTestFactory_csv), "RowTestCases")]
        public int TestReadOfTTRData_XmlRowCount(List<string> fileList, List<string[]> extensionList)
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(fileList, "SHP", extensionList);

            IToolMeasurementData data = reader.Read();

            return data.Results[0].MeasData.Count;
        }





    }
}
