using DataStructures.ToolSpecifications;
using Interfaces.Misc;
using Interfaces.ToolSpecifications;

namespace MeasurementEvaluator.ME_DummyObjectCreator
{
    public class DummyObjectCreator : IDummyObjectCreator
    {
        public void Create(string specificationPath, string referencePath, string measDataPath)
        {

            IToolSpecificationHandler specificationHandler = new ToolSpecification();


        }
    }
}
