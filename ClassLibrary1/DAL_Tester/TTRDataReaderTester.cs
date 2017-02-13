using System;
using System.Collections;
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
        static string folder1 = SetupPath();


        /// <summary>
        /// get the durrent directory and navigate to the test directory
        /// </summary>
        /// <returns></returns>
        static string SetupPath ()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            DirectoryInfo dirName = Directory.GetParent(Directory.GetParent(path.Substring(6, path.Length - 6)).ToString());

            return  dirName.ToString() + @"\DAL_Tester\TTR_InputData";
        }


        public class TTRTestFactory
        {
            public static IEnumerable ColummTestCases
            {
                get
                {
                    //only csv extension
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("TTR ReadData column - only csv 1").Returns(11);

                    //only txt extension
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt" }, new List<string[]> { new string[] { ".txt", "\t" } }).SetName("TTR ReadData column - only txt 1").Returns(11);

                    // only csv
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("TTR ReadData column - only csv 2").Returns(11);

                    // both txt and csv
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" }, new string[] { ".txt", "\t" } }).SetName("TTR ReadData column - both csv and txt").Returns(13);

                    // null extension list
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" }, null).SetName("TTR ReadData column - null extension list").Returns(0);

                    // empty extension list
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" }, new List<string[]>()).SetName("TTR ReadData column - empty extension list").Returns(0);

                    //  null file List
                    yield return new TestCaseData(null, new List<string[]> { new string[] { ".csv", ";" }, new string[] { ".txt", "\t" } }).SetName("TTR ReadData column - null filelist").Returns(0);

                    // empty file list
                    yield return new TestCaseData(new List<string>(), new List<string[]> { new string[] { ".csv", ";" }, new string[] { ".txt", "\t" } }).SetName("TTR ReadData column - empty file list").Returns(0);

                    //  both lists are null
                    yield return new TestCaseData(new List<string>(), new List<string[]>()).SetName("TTR ReadData column - both lists null").Returns(0);

                    // test the reading of a csv file with empty sections
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_3.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("TTR ReadData column - empty number csv").Returns(11);
                }
            }

            public static IEnumerable RowTestCases
            {
                get
                {
                    //only csv extension
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("TTR ReadData row - only csv 1").Returns(10);

                    //only txt extension
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt" }, new List<string[]> { new string[] { ".txt", "\t" } }).SetName("TTR ReadData row - only txt 1").Returns(10);

                    // only csv
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("TTR ReadData row - only csv 2").Returns(10);

                    // both txt and csv
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_1.txt", folder1 + @"\\TTR_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" }, new string[] { ".txt", "\t" } }).SetName("TTR ReadData row - both txt and csv").Returns(20);

                    // test the reading of a csv file with empty sections
                    yield return new TestCaseData(new List<string> { folder1 + @"\\TTR_TestMeasData_3.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("TTR ReadData row - empty number csv").Returns(10);
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="extensionList"></param>
        /// <param name="expectedColumnCount"></param>
        [Test, Category("TTRMeasurementDataReading"), TestCaseSource(typeof(TTRTestFactory), "ColumnTestCases")]
        public int TestReadOfTTRData_ColumnCount(List<string> fileList, List<string[]> extensionList)
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(fileList, "TTR", extensionList );

            IToolMeasurementData data = reader.Read();

            //Assert.AreEqual(expectedColumnCount, data.Results.Count);
            //Assert.AreEqual(expectedRowCount, data.Results[0].MeasData.Count);

            return data.Results.Count;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="extensionList"></param>
        /// <param name="expectedColumnCount"></param>
        [Test, Category("TTRMeasurementDataReading"), TestCaseSource(typeof(TTRTestFactory), "RowTestCases")]
        public int TestReadOfTTRData_RowCount(List<string> fileList, List<string[]> extensionList)
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(fileList, "TTR", extensionList);

            IToolMeasurementData data = reader.Read();

            return data.Results[0].MeasData.Count;
        }


    }
}
