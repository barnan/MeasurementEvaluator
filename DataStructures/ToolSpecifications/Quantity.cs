using Interfaces;
using Interfaces.ToolSpecifications;

namespace DataStructures.ToolSpecifications
{
    public class Quantity : IQuantity
    {
        public Units Dimension { get; }

        public string Name { get; }


        public Quantity(Units dimension, string name)
        {
            Dimension = dimension;
            Name = name;
        }
    }
}
