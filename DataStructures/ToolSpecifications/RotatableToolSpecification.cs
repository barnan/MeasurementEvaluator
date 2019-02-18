using Interfaces;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{
    public class RotatableToolSpecification : ToolSpecification, IRotatableToolSpecificationHandler
    {


        public IReadOnlyList<IReadOnlyList<IQuantity>> Rotations => throw new System.NotImplementedException();



        public SampleOrientation[] GetAvailableOrientations()
        {
            throw new System.NotImplementedException();
        }



        public bool SetCurrentOrientation(SampleOrientation orientation)
        {
            throw new System.NotImplementedException();
        }
    }
}
