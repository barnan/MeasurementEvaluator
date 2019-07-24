using Interfaces.BaseClasses;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{
    public interface IRotatableToolSpecification : IToolSpecification, IRotatable
    {
        /// <summary>
        /// Gives the rotation sequences of all rotatble quantites. For example [0,ThicknessLeft] -> [180, ThicknessRight]
        /// </summary>
        IReadOnlyList<IReadOnlyList<KeyValuePair<SampleOrientation, IQuantity>>> Rotations { get; }
    }


    public interface IRotatableToolSpecificationHandler : IToolSpecificationHandler, IRotatableToolSpecification
    {
        /// <summary>
        /// Gives and sets the rotation sequences of all rotatble quantites
        /// </summary>
        new IReadOnlyList<IReadOnlyList<KeyValuePair<SampleOrientation, IQuantity>>> Rotations { get; set; }
    }

}
