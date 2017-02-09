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


        class SHPColumnTestFactory_csv
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData column - only csv 1").Returns(11);
                }
            }
        }

        class SHPRowTestFactory_csv
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new List<string> { folder1 + @"\\SHP_TestMeasData_2.csv" }, new List<string[]> { new string[] { ".csv", ";" } }).SetName("SHP ReadData row - only csv 1").Returns(10);
                }
            }
        }



        [Test, Category("SHPMeasurementDataReading"), TestCaseSource(typeof(SHPColumnTestFactory_csv), "SHPColumnTestCases")]
        public int TestReadOfTTRData_ColumnCount(List<string> fileList, List<string[]> extensionList)
        {
            IMeasDataFileReader reader = new ToolMeasDataReader( fileList, "SHP", extensionList);

            IToolMeasurementData data = reader.Read();

            return data.Results.Count;
        }




        [Test, Category("SHPMeasurementDataReading"), TestCaseSource(typeof(SHPRowTestFactory_csv), "SHPRowTestCases")]
        public int TestReadOfTTRData_RowCount(List<string> fileList, List<string[]> extensionList)
        {
            IMeasDataFileReader reader = new ToolMeasDataReader(fileList, "SHP", extensionList);

            IToolMeasurementData data = reader.Read();

            return data.Results[0].MeasData.Count;
        }


    }
}
