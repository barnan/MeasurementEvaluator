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
            // thickness specification 01:

            ICondition<double> thicknessAverageAbsCondition = new SimpleCondition()
            {
                Name = "Thickness Average Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.Average,
                LeftValue = 2,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessAverageAbsCondition.CalculationTypeHandler.Relativity = Relativity.Absolute;


            ICondition<double> thicknessAverageRelCondition = new SimpleCondition()
            {
                Name = "Thickness Average Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.Average,
                LeftValue = 1,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessAverageRelCondition.CalculationTypeHandler.Relativity = Relativity.Relative;


            ICondition<double> thicknessStdAbsCondition = new SimpleCondition()
            {
                Name = "Thickness Std Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdAbsCondition.CalculationTypeHandler.Relativity = Relativity.Absolute;


            ICondition<double> thicknessStdRelCondition = new SimpleCondition()
            {
                Name = "Thickness Std Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 0.5,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdRelCondition.CalculationTypeHandler.Relativity = Relativity.Relative;


            // ttv psecification:
            ICondition<double> ttvStdAbsCondition = new SimpleCondition()
            {
                Name = "TTV Std Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 2,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            ttvStdAbsCondition.CalculationTypeHandler.Relativity = Relativity.Absolute;


            // sawmark specification:
            ICondition<double> sawmarkStdAbsCondition = new SimpleCondition()
            {
                Name = "SawMark Std Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 2,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            sawmarkStdAbsCondition.CalculationTypeHandler.Relativity = Relativity.Absolute;


            // resisitivity conditions

            ICondition<double> resistivityAverageRelCondition = new SimpleCondition()
            {
                Name = "Resistivity Average Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.Average,
                LeftValue = 5,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            resistivityAverageRelCondition.CalculationTypeHandler.Relativity = Relativity.Relative;


            ICondition<double> resistivityStdRelCondition = new SimpleCondition()
            {
                Name = "Resistivity Std Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            resistivityStdRelCondition.CalculationTypeHandler.Relativity = Relativity.Relative;


            IQuantitySpecificationHandler thicknessQuantitySpecification = new QuantitySpecification();
            thicknessQuantitySpecification.Quantity = new Quantity(Units.um, "Thickness Average");
            thicknessQuantitySpecification.Conditions = new List<ICondition> { thicknessAverageAbsCondition, thicknessAverageRelCondition, thicknessStdAbsCondition, thicknessStdRelCondition };

            IQuantitySpecificationHandler ttvQuantitySpecification = new QuantitySpecification();
            ttvQuantitySpecification.Quantity = new Quantity(Units.um, "TTV");
            ttvQuantitySpecification.Conditions = new List<ICondition> { ttvStdAbsCondition };

            IQuantitySpecificationHandler sawmarkQuantitySpecification = new QuantitySpecification();
            sawmarkQuantitySpecification.Quantity = new Quantity(Units.um, "Sawmark");
            sawmarkQuantitySpecification.Conditions = new List<ICondition> { sawmarkStdAbsCondition };

            IQuantitySpecificationHandler resistivityQuantitySpecification = new QuantitySpecification();
            resistivityQuantitySpecification.Quantity = new Quantity(Units.um, "Resistivity Average");
            resistivityQuantitySpecification.Conditions = new List<ICondition> { resistivityAverageRelCondition, resistivityStdRelCondition };


            List<IQuantitySpecification> quanSpecList = new List<IQuantitySpecification>
            {
                thicknessQuantitySpecification,
                ttvQuantitySpecification,
                sawmarkQuantitySpecification,
                resistivityQuantitySpecification
            };


            IToolSpecificationHandler specificationHandler = new ToolSpecification
            {
                Name = "TTR Compliant Specification",
                ToolName = ToolNames.TTR,
                QuantitySpecifications = quanSpecList
            };

            readerWriter.WriteToFile(specificationHandler, Path.Combine(specificationPath, $"TTR_Spec_01{_fileExtension}"));


            //-----------------------------------------------------------------------------------------------------------------------------------
            // thickness specification 02:

            ICondition<double> thicknessAverageAbsCondition2 = new SimpleCondition()
            {
                Name = "Thickness Average Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.Average,
                LeftValue = 2,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessAverageAbsCondition2.CalculationTypeHandler.Relativity = Relativity.Absolute;

            ICondition<double> thicknessAverageRelCondition2 = new SimpleCondition()
            {
                Name = "Thickness Average Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.Average,
                LeftValue = 1,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessAverageRelCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            ICondition<double> thicknessStdAbsCondition2 = new SimpleCondition()
            {
                Name = "Thickness Std Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdAbsCondition2.CalculationTypeHandler.Relativity = Relativity.Absolute;

            ICondition<double> thicknessStdRelCondition2 = new SimpleCondition()
            {
                Name = "Thickness Std Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 0.5,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            thicknessStdRelCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            ICondition<double> thicknessCpkCondition2 = new CpkCondition()
            {
                Name = "Thickness Cpk Condition",
                CalculationTypeHandler = CalculationTypeHandler.Cpk,
                LeftValue = 1.33,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                HalfTolerance = 1
            };
            thicknessCpkCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            // ttv psecification:
            ICondition<double> ttvStdAbsCondition2 = new SimpleCondition()
            {
                Name = "TTV Std Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 2,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            ttvStdAbsCondition2.CalculationTypeHandler.Relativity = Relativity.Absolute;


            ICondition<double> ttvCpkCondition2 = new CpkCondition()
            {
                Name = "TTV Cpk Condition",
                CalculationTypeHandler = CalculationTypeHandler.Cpk,
                LeftValue = 1.33,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                HalfTolerance = 2
            };
            ttvCpkCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            // sawmark psecification:
            ICondition<double> sawmarkStdAbsCondition2 = new SimpleCondition()
            {
                Name = "SawMark Std Abs Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 2,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            sawmarkStdAbsCondition2.CalculationTypeHandler.Relativity = Relativity.Absolute;


            ICondition<double> sawmarkCpkCondition2 = new CpkCondition()
            {
                Name = "SawMark Cpk Condition",
                CalculationTypeHandler = CalculationTypeHandler.Cpk,
                LeftValue = 1.33,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                HalfTolerance = 4
            };
            sawmarkCpkCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            // resistivity conditions
            ICondition<double> resistivityAverageRelCondition2 = new SimpleCondition()
            {
                Name = "Resistivity Average Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.Average,
                LeftValue = 5,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            resistivityAverageRelCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            ICondition<double> resistivityStdRelCondition2 = new SimpleCondition()
            {
                Name = "Resistivity Std Rel Condition",
                CalculationTypeHandler = CalculationTypeHandler.StandardDeviation,
                LeftValue = 1,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                ValidIf = RelationHandler.ALLWAYS,
                ValidIf_Value = 0,
            };
            resistivityStdRelCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            ICondition<double> resistivityCpkCondition2 = new CpkCondition()
            {
                Name = "Resistivity Cpk Condition",
                CalculationTypeHandler = CalculationTypeHandler.Cpk,
                LeftValue = 1.33,
                ConditionRelation = RelationHandler.LESSOREQUAL,
                Enabled = true,
                HalfTolerance = 2.5
            };
            resistivityCpkCondition2.CalculationTypeHandler.Relativity = Relativity.Relative;


            IQuantitySpecificationHandler thicknessQuantitySpecification2 = new QuantitySpecification();
            thicknessQuantitySpecification2.Quantity = new Quantity(Units.um, "Thickness Average");
            thicknessQuantitySpecification2.Conditions = new List<ICondition> { thicknessAverageAbsCondition2, thicknessStdAbsCondition2, thicknessStdAbsCondition2, thicknessStdRelCondition2, thicknessCpkCondition2
    };

            IQuantitySpecificationHandler ttvQuantitySpecification2 = new QuantitySpecification();
            ttvQuantitySpecification2.Quantity = new Quantity(Units.um, "TTV");
            ttvQuantitySpecification2.Conditions = new List<ICondition> { ttvStdAbsCondition2, ttvCpkCondition2 };

            IQuantitySpecificationHandler sawmarkQuantitySpecification2 = new QuantitySpecification();
            sawmarkQuantitySpecification2.Quantity = new Quantity(Units.um, "Sawmark");
            sawmarkQuantitySpecification2.Conditions = new List<ICondition> { sawmarkStdAbsCondition2, sawmarkCpkCondition2 };


            IQuantitySpecificationHandler resistivityQuantitySpecification2 = new QuantitySpecification();
            resistivityQuantitySpecification2.Quantity = new Quantity(Units.um, "Resistivity Average");
            resistivityQuantitySpecification2.Conditions = new List<ICondition> { resistivityAverageRelCondition2, resistivityStdRelCondition2, resistivityCpkCondition2 };

            List<IQuantitySpecification> quanSpecList2 = new List<IQuantitySpecification>
            {
                thicknessQuantitySpecification2,
                ttvQuantitySpecification2,
                sawmarkQuantitySpecification2,
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
