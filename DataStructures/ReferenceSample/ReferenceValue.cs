using Interfaces;
using Interfaces.ReferenceSample;

namespace DataStructures.ReferenceSample
{
    public class ReferenceValue : IReferenceValue
    {
        public string Name { get; }

        public Units Dimension { get; }


        public ReferenceValue()
        {
        }


        public ReferenceValue(string name, Units dim, double val)
        {
            Name = name;
            Dimension = dim;
        }

    }


    public class ReferenceValue<T> : ReferenceValue, IReferenceValue<T> where T : struct
    {
        public T Value { get; }


        public ReferenceValue()
        {
        }

        public ReferenceValue(T val)
        {
            Value = val;
        }
    }


}
