using BaseClasses;
using Interfaces.Misc;

namespace ToolSpecificInterfaces.MeasurementEvaluator.ReferenceSample
{

    public interface IReferenceValue : INamed
    {
        /// <summary>
        /// Dimension of the Value
        /// </summary>
        Units Dimension { get; }
    }

    public interface IReferenceValue<T> : IReferenceValue
        where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        T Value { get; }
    }



    public interface IReferenceValueHandler : IReferenceValue, INamedHandler
    {
        new Units Dimension { get; set; }
    }

    public interface IReferenceValueHandler<T> : IReferenceValueHandler, IReferenceValue<T>
        where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        new T Value { get; set; }
    }


}
