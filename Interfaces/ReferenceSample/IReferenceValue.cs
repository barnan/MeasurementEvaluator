using System;

namespace Interfaces.ReferenceSample
{

    public interface IReferenceValue : IComparable<IReferenceValue>
    {
        /// <summary>
        /// value name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Dimension of the Value
        /// </summary>
        Units Dimension { get; }
    }

    public interface IReferenceValue<T> : IReferenceValue where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        T Value { get; }
    }



    public interface IReferenceValueHandler : IReferenceValue
    {
        new string Name { get; set; }

        new Units Dimension { get; set; }
    }

    public interface IReferenceValueHandler<T> : IReferenceValue<T> where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        T Value { get; set; }
    }


}
