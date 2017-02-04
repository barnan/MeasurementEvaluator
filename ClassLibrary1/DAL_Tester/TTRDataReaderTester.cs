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
    class TTRDataReaderTester
    {
        /// <summary>
        /// contains the 
        /// </summary>
        string folder1;

        [OneTimeSetUp]
        public void SetInputDir()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            DirectoryInfo dirName = Directory.GetParent(Directory.GetParent(path.Substring(6, path.Length - 6)).ToString());

            folder1 = dirName.ToString() + "\\DAL_Tester\\TTR_InputData";
        }


        [Test, Category("MeasurementDataReading")]
        public void TestNumberOfColumnsAndRows()
        {
            //IMeasurementDataReader reader = new TTRMeasurementDataReader(new List<string> { new  });


        }

    }
}
