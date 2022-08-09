using System.Xml.Linq;
using BaseClasses.MeasurementEvaluator;
using Interfaces.MeasurementEvaluator.ReferenceSample;

namespace MeasurementDataStructures.Reference
{
    public class ReferenceValue : IReferenceValueHandler<double>
    {
        public string Name { get; set; }

        public Units Dimension { get; set; }

        public double Value { get; set; }

        public ReferenceValue()
        {
        }


        public ReferenceValue(string name, Units dim, double val)
        {
            Name = name;
            Dimension = dim;
            Value = val;
        }

        public XElement SaveToXml(XElement inputElement)
        {
            //this.TrySave(Name, inputElement, nameof(Name));
            //this.TrySave(Dimension, inputElement, nameof(Dimension));
            //this.TrySave(Value, inputElement, nameof(Value));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            //this.TryLoad(inputElement, nameof(Name));
            //this.TryLoad(inputElement, nameof(Dimension));
            //this.TryLoad(inputElement, nameof(Value));
            return true;
        }
    }
}
