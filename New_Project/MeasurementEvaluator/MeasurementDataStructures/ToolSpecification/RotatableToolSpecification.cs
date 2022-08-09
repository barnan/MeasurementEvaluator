using BaseClasses.MeasurementEvaluator;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification
{
    public class RotatableToolSpecification : ToolSpecification, IRotatableToolSpecificationHandler
    {

        public IReadOnlyList<IReadOnlyList<KeyValuePair<SampleOrientations, IQuantity>>> Rotations
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }


        public SampleOrientations[] GetAvailableOrientations()
        {
            throw new System.NotImplementedException();
        }


        public bool SetCurrentOrientation(SampleOrientations orientation)
        {
            throw new System.NotImplementedException();
        }
    }
}
