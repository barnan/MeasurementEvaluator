using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{
    public interface IRotatable
    {
        bool SetCurrentOrientation(SampleOrientation orientation);

        SampleOrientation[] GetAvailableOrientations();

        IReadOnlyList<IReadOnlyList<IQuantity>> Rotations { get; }
    }
}
