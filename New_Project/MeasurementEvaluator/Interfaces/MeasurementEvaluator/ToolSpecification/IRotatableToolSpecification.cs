
using BaseClasses.MeasurementEvaluator;

namespace Interfaces.MeasurementEvaluator.ToolSpecification
{
    public interface IRotatableToolSpecification : IToolSpecification, IRotatable
    {
        /// <summary>
        /// Gives the rotation sequences of all rotatable quantities. For example [0,ThicknessLeft] -> [180, ThicknessRight]
        /// </summary>
        IReadOnlyList<IReadOnlyList<KeyValuePair<SampleOrientations, IQuantity>>> Rotations { get; }
    }


    public interface IRotatableToolSpecificationHandler : IToolSpecificationHandler, IRotatableToolSpecification
    {
        /// <summary>
        /// Gives and sets the rotation sequences of all rotatable quantities
        /// </summary>
        new IReadOnlyList<IReadOnlyList<KeyValuePair<SampleOrientations, IQuantity>>> Rotations { get; set; }
    }
}
