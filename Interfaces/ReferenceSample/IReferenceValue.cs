namespace Interfaces.ReferenceSample
{

    public interface IReferenceValue
    {
        /// <summary>
        /// value name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Dimension of the Value
        /// </summary>
        Units Dimension { get; }

        /// <summary>
        /// number value
        /// </summary>
        double Value { get; }

    }

    public interface IReferenceValueHandler : IReferenceValue
    {
        new string Name { get; set; }

        new Units Dimension { get; set; }

        new double Value { get; set; }

    }


}
