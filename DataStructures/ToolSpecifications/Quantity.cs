using Interfaces;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System.Xml.Linq;

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

        public XElement SaveToXml(XElement inputElement)
        {
            this.TrySave(Dimension, inputElement, nameof(Dimension));
            this.TrySave(Name, inputElement, nameof(Name));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            throw new System.NotImplementedException();
        }
    }
}
