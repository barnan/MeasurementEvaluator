using Interfaces;
using Interfaces.ReferenceSample;
using System;

namespace DataStructures.ReferenceSample
{
    public class ReferenceValue : IReferenceValueHandler
    {
        public string Name { get; set; }

        public Units Dimension { get; set; }


        public ReferenceValue()
        {
        }


        public ReferenceValue(string name, Units dim, double val)
        {
            Name = name;
            Dimension = dim;
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
    }


    public class ReferenceValue<T> : ReferenceValue, IReferenceValue<T> where T : struct
    {
        public T Value { get; }

        public ReferenceValue(T val)
        {
            Value = val;
        }
    }


}
