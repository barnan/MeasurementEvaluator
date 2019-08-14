using DataStructures.ToolSpecifications;
using Interfaces.BaseClasses;
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

            ICondition<double> thicknessStdAbsCondition = new SimpleCondition()
            {
                Name = "Thickness Average Std Abs Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 2,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdAbsCondition.CalculationType.Relativity = Relativity.Absolute;

            ICondition<double> thicknessStdRelCondition = new SimpleCondition()
            {
                Name = "Thickness Average Std Rel Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 0.5,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdRelCondition.CalculationType.Relativity = Relativity.Relative;

            // resisitivity conditions

            ICondition<double> resistivityStdRelCondition = new SimpleCondition()
            {
                Name = "Resistivity Std Rel Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };
            resistivityStdRelCondition.CalculationType.Relativity = Relativity.Relative;


            IQuantitySpecificationHandler thicknessQuantitySpecification = new QuantitySpecification();
            thicknessQuantitySpecification.Quantity = new Quantity(Units.um, "Thickness Average");
            thicknessQuantitySpecification.Conditions = new List<ICondition> { thicknessStdAbsCondition, thicknessStdRelCondition };

            IQuantitySpecificationHandler resistivityQuantitySpecification = new QuantitySpecification();
            resistivityQuantitySpecification.Quantity = new Quantity(Units.um, "Resistivity Average");
            resistivityQuantitySpecification.Conditions = new List<ICondition> { resistivityStdRelCondition };


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

            ICondition<double> thicknessStdAbsCondition2 = new SimpleCondition()
            {
                Name = "Thickness Average Std Abs Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 3,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdAbsCondition2.CalculationType.Relativity = Relativity.Absolute;

            ICondition<double> thicknessStdRelCondition2 = new SimpleCondition()
            {
                Name = "Thickness Average Std Rel Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdRelCondition2.CalculationType.Relativity = Relativity.Relative;

            ICondition<double> thicknessCpkCondition2 = new CpkCondition()
            {
                Name = "Thickness Average Cpk Calculation",
                CalculationType = CalculationTypes.Cpk,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                HalfTolerance = 1
            };

            // resisitivity conditions

            ICondition<double> resistivityStdRelCondition2 = new SimpleCondition()
            {
                Name = "Resistivity Std Rel Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };
            resistivityStdRelCondition2.CalculationType.Relativity = Relativity.Relative;

            ICondition<double> resistivityCpkCondition2 = new CpkCondition()
            {
                Name = "Resistivity Std Cpk Condition",
                CalculationType = CalculationTypes.Cpk,
                LeftValue = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                HalfTolerance = 1
            };




            IQuantitySpecificationHandler thicknessQuantitySpecification2 = new QuantitySpecification();
            thicknessQuantitySpecification2.Quantity = new Quantity(Units.um, "Thickness Average");
            thicknessQuantitySpecification2.Conditions = new List<ICondition> { thicknessStdAbsCondition2, thicknessStdRelCondition2, thicknessCpkCondition2 };

            IQuantitySpecificationHandler resistivityQuantitySpecification2 = new QuantitySpecification();
            resistivityQuantitySpecification2.Quantity = new Quantity(Units.um, "Resistivity Average");
            resistivityQuantitySpecification2.Conditions = new List<ICondition> { resistivityStdRelCondition2, resistivityCpkCondition2 };

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
