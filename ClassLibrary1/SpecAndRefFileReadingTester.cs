using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;
using NUnit.Framework;

namespace ClassLibrary1
{
    [TestFixture]
    class SpecAndRefFileReadingTester
    {
        string folder1;
        string dirName;
        ToolSpecification _tsOriginal;
        ToolSpecification _tsCheck;
        ReferenceSample _rsOriginal;
        ReferenceSample _rsCheck;


        [OneTimeSetUp]
        public void InitTestEnvironment()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            dirName = path.Substring(6, path.Length - 6);

            folder1 = dirName + "\\TestDirectory1";

            Directory.CreateDirectory(folder1);

            //Specification file:
            _tsCheck = new ToolSpecification();
            _tsOriginal = new ToolSpecification("TTR", new List<IQuantitySpecification>
                        {
                            new QuantitySpecification("Thickness",
                                                        new UsualCondition(11, "um", Relation.LESS, true, Relation.LESSOREQUAL, 101),
                                                        new UsualCondition(12, "um", Relation.GREATER, true, Relation.LESSOREQUAL, 102),
                                                        new UsualCondition(13, "um", Relation.EQUAL, true, Relation.LESSOREQUAL, 103),
                                                        new UsualCondition(14, "um", Relation.LESSOREQUAL, true, Relation.LESSOREQUAL, 104),
                                                        new CpkCondition(20, Relation.LESSOREQUAL, RELATIVEORABSOLUTE.ABSOLUTE, true, 7)
                                                     )
                        });

            // reference file:
            _rsCheck = new ReferenceSample();
            _rsOriginal = new ReferenceSample("TTR-001",
                                             new List<IReferenceValue>
                                             {
                                                 new ReferenceValue("Thickness", "um", 200)
                                             }, 
                                             Orientation.Orientation1);


            
        }


        [OneTimeTearDown]
        public void DestroyTestEnvironment()
        {
            Directory.Delete(folder1, true);
        }

        [Test]
        public void ReferenceFileSerializationTest()
        {
            Assert.DoesNotThrow(() => ReferenceSample.Save(folder1 + "\\TTR-001.refTTR", _rsOriginal));
            Assert.DoesNotThrow(() => _rsCheck = ReferenceSample.Read(folder1 + "\\TTR-001.refTTR"));

            Assert.AreEqual(_rsOriginal.SampleID, _rsCheck.SampleID);
            Assert.AreEqual(_rsOriginal.ListOfReferenceValues.Count, _rsCheck.ListOfReferenceValues.Count);

            for ( int i = 0; i < _rsOriginal.ListOfReferenceValues.Count; i++)
            {
                Assert.AreEqual(_rsOriginal.ListOfReferenceValues[i].Name, _rsCheck.ListOfReferenceValues[i].Name);
                Assert.AreEqual(_rsOriginal.ListOfReferenceValues[i].Value, _rsCheck.ListOfReferenceValues[i].Value);
                Assert.AreEqual(_rsOriginal.ListOfReferenceValues[i].Dimension, _rsCheck.ListOfReferenceValues[i].Dimension);
            }
        }

        [Test]
        public void SpecificationFileSerializationTest()
        {
            Assert.DoesNotThrow(() => ToolSpecification.Save(folder1 + "\\TTR_spec.specTTR", _tsOriginal));
            Assert.DoesNotThrow(() => _tsCheck = ToolSpecification.Read(folder1 + "\\TTR_spec.specTTR"));

            Assert.AreEqual(_tsOriginal.ToolName, _tsCheck.ToolName);
            Assert.AreEqual(_tsOriginal.ToolspecificationList.Count, _tsCheck.ToolspecificationList.Count);

            for (int i = 0; i < _tsOriginal.ToolspecificationList.Count; i++)
            {
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].QuantityName, _tsCheck.ToolspecificationList[i].QuantityName);

                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].AccurRelative.ConditionRel, _tsCheck.ToolspecificationList[i].AccurRelative.ConditionRel);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].AccurRelative.Dimension, _tsCheck.ToolspecificationList[i].AccurRelative.Dimension);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].AccurRelative.Valid, _tsCheck.ToolspecificationList[i].AccurRelative.Valid);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].AccurRelative.ValidIfRelation_Relation, _tsCheck.ToolspecificationList[i].AccurRelative.ValidIfRelation_Relation);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].AccurRelative.ValidIfRelation_Value, _tsCheck.ToolspecificationList[i].AccurRelative.ValidIfRelation_Value);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].AccurRelative.Value, _tsCheck.ToolspecificationList[i].AccurRelative.Value);


                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].AccurAbsolute.Value, _tsCheck.ToolspecificationList[i].AccurAbsolute.Value);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].StdAbsolute.Value, _tsCheck.ToolspecificationList[i].StdAbsolute.Value);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].StdRelative.Value, _tsCheck.ToolspecificationList[i].StdRelative.Value);

                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].Cpk.HalfTolerance, _tsCheck.ToolspecificationList[i].Cpk.HalfTolerance);
                Assert.AreEqual(_tsOriginal.ToolspecificationList[i].Cpk.RelOrAbs, _tsCheck.ToolspecificationList[i].Cpk.RelOrAbs);

            }
        }

    }
}
