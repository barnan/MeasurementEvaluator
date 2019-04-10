using DataStructures.ToolSpecifications;
using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace ME_DummyObjectCreator
{
    internal class SpecificationCreator
    {
        internal void Create(string specificationPath, IHDDFileReaderWriter readerWriter)
        {
            ICondition<double> condition1 = new SimpleCondition()
            {
                Name = "Thickness Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                Value = 1,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                RelOrAbs = RELATIVEORABSOLUTE.ABSOLUTE,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> condition2 = new SimpleCondition()
            {
                Name = "Thickness Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                Value = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = RELATIVEORABSOLUTE.RELATIVE,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            // resisitivity conditions
            ICondition<double> condition3 = new SimpleCondition()
            {
                Name = "Resistivity Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                Value = 1,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                RelOrAbs = RELATIVEORABSOLUTE.ABSOLUTE,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> condition4 = new SimpleCondition()
            {
                Name = "Resitivity Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                Value = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                RelOrAbs = RELATIVEORABSOLUTE.RELATIVE,
                ValidIf = Relations.ALLWAYS,
                ValidIf_Value = 0,
            };



            IQuantitySpecificationHandler thicknessQuantitySpecification = new QuantitySpecification();
            thicknessQuantitySpecification.Quantity = new Quantity(Units.um, "Thickness Average");
            thicknessQuantitySpecification.Conditions = new List<ICondition> { condition1, condition2 };

            IQuantitySpecificationHandler resistivityQuantitySpecification = new QuantitySpecification();
            resistivityQuantitySpecification.Quantity = new Quantity(Units.um, "Resistivity Average");
            resistivityQuantitySpecification.Conditions = new List<ICondition> { condition3, condition4 };


            List<IQuantitySpecification> quanSpecList = new List<IQuantitySpecification>
            {
                thicknessQuantitySpecification,
                resistivityQuantitySpecification
            };


            IToolSpecificationHandler specificationHandler = new ToolSpecification
            {
                Name = "TTR Coompliant Specification",
                ToolName = ToolNames.TTR,
                Specifications = quanSpecList
            };


            readerWriter.WriteToFile<IToolSpecification>(specificationHandler, specificationPath);
        }


    }
}
