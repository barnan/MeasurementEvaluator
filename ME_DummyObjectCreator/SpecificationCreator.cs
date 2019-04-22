using DataStructures.ToolSpecifications;
using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace ME_DummyObjectCreator
{
    internal class SpecificationCreator
    {
        private string _fileExtension = ".Spec";


        internal void Create(string specificationPath, IHDDFileReaderWriter readerWriter)
        {
            ICondition<double> condition1 = new SimpleCondition()
            {
                Name = "Thickness Average Avg Condition",
                CalculationType = CalculationTypes.Average,
                Value = 1,
                ConditionRelation = Relations.EQUAL,
                Enabled = true,
                RelOrAbs = Relativity.Absolute,
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
                RelOrAbs = Relativity.Relative,
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
                RelOrAbs = Relativity.Absolute,
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
                RelOrAbs = Relativity.Relative,
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
                Name = "TTR Compliant Specification",
                ToolName = ToolNames.TTR,
                Specifications = quanSpecList
            };


            XElement specificationElement = new XElement(nameof(ToolSpecification));
            XElement output = specificationHandler.SaveToXml(specificationElement);

            output.Save(Path.Combine(specificationPath, $"TTR_Spec_1{_fileExtension}"));
        }
    }
}
