using Interfaces;
using Interfaces.ReferenceSample;
using Miscellaneous;
using System;
using System.Xml.Linq;

namespace DataStructures.ReferenceSample
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


        public int CompareTo(IReferenceValue other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public XElement SaveToXml(XElement inputElement)
        {
            this.TrySave(Name, inputElement, nameof(Name));
            this.TrySave(Dimension, inputElement, nameof(Dimension));
            this.TrySave(Value, inputElement, nameof(Value));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            this.TryLoad(inputElement, nameof(Name));
            this.TryLoad(inputElement, nameof(Dimension));
            this.TryLoad(inputElement, nameof(Value));
            return true;
        }
    }


}
