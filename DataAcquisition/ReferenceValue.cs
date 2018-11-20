using Interfaces.ReferenceSample;

namespace DataAcquisition
{
    public class ReferenceValue : IReferenceValue
    {
        public string Name { get; set; }
        public string Dimension { get; set; }
        public double Value { get; set; }

        public ReferenceValue()
        {
        }

        public ReferenceValue(string name, string dim, double val)
        {
            Name = name;
            Dimension = dim;
            Value = val;
        }

    }
}
