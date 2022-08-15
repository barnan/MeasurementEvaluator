using System.Xml.Linq;
using Interfaces;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification
{
    public class Quantity : IQuantityHandler
    {

        public Units Dimension { get; set; }

        /// <summary>
        /// name of the quantity. E.g.: Thickness, Resistivity
        /// </summary>
        public string Name { get; set; }


        public Quantity(Units dimension, string name)
        {
            Dimension = dimension;
            Name = name;
        }

       
        public XElement SaveToXml(XElement inputElement)
        {
            //this.TrySave(Name, inputElement, nameof(Name));
            //this.TrySave(Dimension, inputElement, nameof(Dimension));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            //this.TryLoad(inputElement, nameof(Name));
            //this.TryLoad(inputElement, nameof(Dimension));
            return true;
        }
    }
}
