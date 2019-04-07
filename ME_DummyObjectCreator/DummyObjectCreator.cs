using DataStructures.ToolSpecifications;
using Interfaces;
using Interfaces.Misc;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace MeasurementEvaluator.ME_DummyObjectCreator
{
    public class DummyObjectCreator : IDummyObjectCreator
    {
        public void Create(string specificationPath, string referencePath, string measDataPath)
        {
            // thickness avg conditions:
            ICondition<double> condition1 = new SimpleCondition()
            {
                Name = "Thickness Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                Value = 1,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                ValidIf = ValidIfRelations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> condition2 = new SimpleCondition()
            {
                Name = "Thickness Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                Value = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = ValidIfRelations.ALLWAYS,
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
                ValidIf = ValidIfRelations.ALLWAYS,
                ValidIf_Value = 0,
            };

            ICondition<double> condition4 = new SimpleCondition()
            {
                Name = "Resitivity Average Std Condition",
                CalculationType = CalculationTypes.StandardDeviation,
                Value = 1,
                ConditionRelation = Relations.LESSOREQUAL,
                Enabled = true,
                ValidIf = ValidIfRelations.ALLWAYS,
                ValidIf_Value = 0,
            };





            IQuantitySpecificationHandler quantitySpecification = new QuantitySpecification();
            quantitySpecification.Quantity = new Quantity(Units.um, "Thickness Average");
            quantitySpecification.Conditions = new List<ICondition> { condition1, condition2 };


            List<IQuantitySpecification> quanSpecList = new List<IQuantitySpecification>
            {
                quantitySpecification
            };


            IToolSpecificationHandler specificationHandler = new ToolSpecification
            {
                Name = "TTR Coompliant Specification",
                ToolName = ToolNames.TTR,
                Specifications = quanSpecList
            };


        }
    }
}
