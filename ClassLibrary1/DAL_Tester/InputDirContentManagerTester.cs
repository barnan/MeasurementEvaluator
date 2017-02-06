using System;
using System.Collections.Generic;
using System.IO;
using Measurement_Evaluator.DAL;
using NUnit.Framework;

namespace ClassLibrary1.DAL_Tester
{
    [TestFixture]
    class InputDirContentManagerTester
    {
        string _folder1;
        string _folder2;
        string _folder3;

        #region init and close
        [OneTimeSetUp]
        public void InitTestEnvironment()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string dirName = path.Substring(6, path.Length - 6);

            _folder1 = dirName + "\\SpecFiles";
            _folder2 = dirName + "\\RefFiles";
            _folder3 = dirName + "\\EmptyDir";

            Directory.CreateDirectory(_folder1);
            Directory.CreateDirectory(_folder2);
            Directory.CreateDirectory(_folder3);

            // create specification files:
            File.Create(_folder1 + "\\file01.specSHP").Dispose();
            File.Create(_folder1 + "\\file02.specSHP").Dispose();
            File.Create(_folder1 + "\\file03.specSHP").Dispose();

            File.Create(_folder1 + "\\file01.specTTR").Dispose();
            File.Create(_folder1 + "\\file02.specTTR").Dispose();
            File.Create(_folder1 + "\\file03.specTTR").Dispose();
            File.Create(_folder1 + "\\file04.specTTR").Dispose();

            File.Create(_folder1 + "\\file01.specPLI").Dispose();
            File.Create(_folder1 + "\\file02.specPLI").Dispose();
            File.Create(_folder1 + "\\file03.specPLI").Dispose();
            File.Create(_folder1 + "\\file04.specPLI").Dispose();
            File.Create(_folder1 + "\\file05.specPLI").Dispose();

            File.Create(_folder1 + "\\random01").Dispose();
            File.Create(_folder1 + "\\random02").Dispose();
            File.Create(_folder1 + "\\random03").Dispose();

            File.Create(_folder1 + "\\file01.exe").Dispose();

            File.Create(_folder1 + "\\file01.txt").Dispose();
            File.Create(_folder1 + "\\file02.txt").Dispose();

            // create reference files:
            File.Create(_folder2 + "\\file01.refSHP").Dispose();
            File.Create(_folder2 + "\\file02.refSHP").Dispose();
            File.Create(_folder2 + "\\file03.refSHP").Dispose();
            File.Create(_folder2 + "\\file04.refSHP").Dispose();

            File.Create(_folder2 + "\\file01.refTTR").Dispose();
            File.Create(_folder2 + "\\file02.refTTR").Dispose();
            File.Create(_folder2 + "\\file03.refTTR").Dispose();

            File.Create(_folder2 + "\\file01.refPLI").Dispose();
            File.Create(_folder2 + "\\file02.refPLI").Dispose();
            File.Create(_folder2 + "\\file03.refPLI").Dispose();
            File.Create(_folder2 + "\\file04.refPLI").Dispose();
            File.Create(_folder2 + "\\file05.refPLI").Dispose();

            File.Create(_folder2 + "\\random01").Dispose();
            File.Create(_folder2 + "\\random02").Dispose();

            File.Create(_folder2 + "\\file01.exe").Dispose();

            File.Create(_folder2 + "\\file01.txt").Dispose();
            File.Create(_folder2 + "\\file02.txt").Dispose();
        }

        [OneTimeTearDown]
        public void DestroyTestEnvironment()
        {
            try
            {
                Directory.Delete(_folder1, true);
                Directory.Delete(_folder2, true);
                Directory.Delete(_folder3, true);
            }
            catch (Exception ex)
            {
                // errror in the file deleting
            }
        }
        #endregion


        #region spec test

        /// <summary>
        /// Checks the number of extensions in the folder:
        /// </summary>
        [Test, Category("SpecificationInputDir")]
        public void TestSpecFileListReading_01()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder1, "spec");
            int expectedExtensionCount = 3;
            Assert.AreEqual(expectedExtensionCount, fileList_Spec.Count);
        }

        /// <summary>
        /// Checks the number of SHP files in the specification folder
        /// </summary>
        [Test, Category("SpecificationInputDir")]
        public void TestSpecFileListReading_02()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder1, "spec");

            var list = fileList_Spec.Find(p => Path.GetExtension(p[0]).Contains("SHP"));
            int expectedCount = 3;
            Assert.AreEqual(expectedCount, list.Count);
        }

        /// <summary>
        /// Checks the number of SHP files in the specification folder
        /// </summary>
        [Test, Category("SpecificationInputDir")]
        public void TestSpecFileListReading_03()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder1, "spec");

            var list = fileList_Spec.Find(p => Path.GetExtension(p[0]).Contains("PLI"));
            int expectedCount = 5;
            Assert.AreEqual(expectedCount, list.Count);
        }

        /// <summary>
        /// Checks the number of "TTR" extensions in the spec folder:
        /// </summary>
        [Test, Category("SpecificationInputDir")]
        public void TestSpecFileListReading_04()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder1, "spec");

            var list = fileList_Spec.Find(p => Path.GetExtension(p[0]).Contains("TTR"));
            int expectedCount = 4;
            Assert.AreEqual(expectedCount, list.Count);
        }

        /// <summary>
        /// Checks the reading of the empty directory:
        /// </summary>
        [Test, Category("SpecificationInputDir")]
        public void TestSpecFileListReading_05()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder3, "spec");

            int expectedCount = 0;
            Assert.AreEqual(expectedCount, fileList_Spec.Count);
        }

        #endregion

        #region ref test

        /// <summary>
        /// Checks the number of extensions in the reference folder:
        /// </summary>
        [Test, Category("ReferenceInputDir")]
        public void TestRefFileListReading_11()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder2, "ref");
            int expectedExtensionCount = 3;
            Assert.AreEqual(expectedExtensionCount, fileList_Spec.Count);
        }


        /// <summary>
        /// Checks the number of SHP files in the reference folder
        /// </summary>
        [Test, Category("ReferenceInputDir")]
        public void TestRefFileListReading_12()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder2, "ref");

            var list = fileList_Spec.Find(p => Path.GetExtension(p[0]).Contains("SHP"));
            int expectedCount = 4;
            Assert.AreEqual(expectedCount, list.Count);
        }

        /// <summary>
        /// Checks the number of SHP files in the reference file folder
        /// </summary>
        [Test, Category("ReferenceInputDir")]
        public void TestRefFileListReading_13()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder2, "ref");

            var list = fileList_Spec.Find(p => Path.GetExtension(p[0]).Contains("PLI"));
            int expectedCount = 5;
            Assert.AreEqual(expectedCount, list.Count);
        }

        /// <summary>
        /// Checks the number of "TTR" extensions in the reference folder:
        /// </summary>
        [Test, Category("ReferenceInputDir")]
        public void TestRefFileListReading_14()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();
            List<List<string>> fileList_Spec = idcm.GetDirectoryContent(_folder2, "ref");

            var list = fileList_Spec.Find(p => Path.GetExtension(p[0]).Contains("TTR"));
            int expectedCount = 3;
            Assert.AreEqual(expectedCount, list.Count);
        }

        #endregion


        #region empty folder test:

        /// <summary>
        /// Checks the number of extensions in the empty folder:
        /// </summary>
        [Test, Category("Empty folder")]
        public void TestEmptyFileListReading_11()
        {
            InputConfigDirectoryContentManager idcm = new InputConfigDirectoryContentManager();

            List<List<string>> fileList_Spec = null;

            Assert.DoesNotThrow(() => fileList_Spec = idcm.GetDirectoryContent(_folder3, "spec"));

            int expectedExtensionCount = 0;
            Assert.AreEqual(expectedExtensionCount, fileList_Spec.Count);
        }

        #endregion

    }
}
