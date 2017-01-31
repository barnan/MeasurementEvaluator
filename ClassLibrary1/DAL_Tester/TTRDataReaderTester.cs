using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ClassLibrary1.DAL_Tester
{
    [TestFixture]
    class TTRDataReaderTester
    {
        string folder1;

        [OneTimeSetUp]
        public void CopyFiles()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            DirectoryInfo dirName = Directory.GetParent(Directory.GetParent(path.Substring(6, path.Length - 6)).ToString());

            folder1 = dirName.ToString() + "\\DAL_Tester\\TTR_InputData";



        }

        
        [OneTimeTearDown]
        public void DeleteFiles()
        {


        }



        [Test, Category("MeasurementDataReading")]
        public void TestNumberOfColumnsAndRows()
        {
            


        }

    }
}
