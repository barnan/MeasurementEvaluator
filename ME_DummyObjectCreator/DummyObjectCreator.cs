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



            IQuantitySpecificationHandler quantitySpecification = new QuantitySpecification();
            quantitySpecification.Quantity = new Quantity(Units.um, "Thisckness Average");
            quantitySpecification.Conditions


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
