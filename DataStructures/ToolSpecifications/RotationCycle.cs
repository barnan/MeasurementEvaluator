using Interfaces.ReferenceSample;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{
    public class RotationCycle
    {
        private List<IReferenceValue> _cycle;
        public IReadOnlyList<IReferenceValue> Cycle => _cycle.AsReadOnly();

        public RotationCycle()
        {

        }


    }
}
