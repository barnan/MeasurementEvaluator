using DataStructures.ToolSpecifications;
using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;
using System.IO;

namespace ME_DummyObjectCreator
{
    internal class SpecificationCreator
    {
        private string _fileExtension = ".Spec";


        internal void Create(string specificationPath, IHDDFileReaderWriter readerWriter)
        {
            // ttr specification 01:

            ICondition<double> thicknessAVGCondition = new SimpleCondition()
            {
                Name = "Thickness Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                LeftValue = 200,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Absolute,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> thicknessSTDCondition = new SimpleCondition()
            {
                Name = "Thickness Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 0.5,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Relative,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            // resisitivity conditions
            ICondition<double> resistivityAVGCondition = new SimpleCondition()
            {
                Name = "Resistivity Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                LeftValue = 1.5,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Absolute,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> resistivitySTDCondition = new SimpleCondition()
            {
                Name = "Resitivity Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Relative,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };


            IQuantitySpecificationHandler thicknessQuantitySpecification = new QuantitySpecification();
            thicknessQuantitySpecification.Quantity = new Quantity(Units.um, "Thickness Average");
            thicknessQuantitySpecification.Conditions = new List<ICondition> { thicknessAVGCondition, thicknessSTDCondition };

            IQuantitySpecificationHandler resistivityQuantitySpecification = new QuantitySpecification();
            resistivityQuantitySpecification.Quantity = new Quantity(Units.um, "Resistivity Average");
            resistivityQuantitySpecification.Conditions = new List<ICondition> { resistivityAVGCondition, resistivitySTDCondition };


            List<IQuantitySpecification> quanSpecList = new List<IQuantitySpecification>
            {
                thicknessQuantitySpecification,
                resistivityQuantitySpecification
            };


            IToolSpecificationHandler specificationHandler = new ToolSpecification
            {
                Name = "TTR Compliant Specification",
                ToolName = ToolNames.TTR,
                QuantitySpecifications = quanSpecList
            };

            readerWriter.WriteToFile(specificationHandler, Path.Combine(specificationPath, $"TTR_Spec_01{_fileExtension}"));



            // ttr specification 02:

            ICondition<double> thicknessAVGCondition2 = new SimpleCondition()
            {
                Name = "Thickness Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                LeftValue = 201,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Absolute,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> thicknessSTDCondition2 = new SimpleCondition()
            {
                Name = "Thickness Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Relative,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> thicknessCpkCondition2 = new CpkCondition()
            {
                Name = "Thickness Cpk Calculation",
                CalculationType = CalculationTypes.Cpk,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Relative,
                HalfTolerance = 1
            };

            // resisitivity conditions
            ICondition<double> resistivityAVGCondition2 = new SimpleCondition()
            {
                Name = "Resistivity Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                LeftValue = 1.7,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Absolute,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> resistivitySTDCondition2 = new SimpleCondition()
            {
                Name = "Resitivity Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Relative,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> resistivityCpkCondition2 = new CpkCondition()
            {
                Name = "Resitivity Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Relative,
                HalfTolerance = 1
            };



            IQuantitySpecificationHandler thicknessQuantitySpecification2 = new QuantitySpecification();
            thicknessQuantitySpecification2.Quantity = new Quantity(Units.um, "Thickness Average");
            thicknessQuantitySpecification2.Conditions = new List<ICondition> { thicknessAVGCondition2, thicknessSTDCondition2, thicknessCpkCondition2 };

            IQuantitySpecificationHandler resistivityQuantitySpecification2 = new QuantitySpecification();
            resistivityQuantitySpecification2.Quantity = new Quantity(Units.um, "Resistivity Average");
            resistivityQuantitySpecification2.Conditions = new List<ICondition> { resistivityAVGCondition2, resistivitySTDCondition2, resistivityCpkCondition2 };

            List<IQuantitySpecification> quanSpecList2 = new List<IQuantitySpecification>
            {
                thicknessQuantitySpecification2,
                resistivityQuantitySpecification2
            };


            IToolSpecificationHandler specificationHandler2 = new ToolSpecification
            {
                Name = "TTR Strict Specification",
                ToolName = ToolNames.TTR,
                QuantitySpecifications = quanSpecList2
            };

            readerWriter.WriteToFile(specificationHandler2, Path.Combine(specificationPath, $"TTR_Spec_02{_fileExtension}"));



        }
    }
}
