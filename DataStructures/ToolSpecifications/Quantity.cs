using Interfaces;
using Interfaces.ToolSpecifications;

namespace DataStructures.ToolSpecifications
{
    public class Quantity : IQuantityHandler
    {

        public Units Dimension { get; set; }

        public string Name { get; set; }


        public Quantity(Units dimension, string name)
        {
            Dimension = dimension;
            Name = name;
        }
    }
}
