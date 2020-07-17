using BaseClasses;
using Interfaces.Misc;

namespace ToolSpecificInterfaces.MeasurementEvaluator.ReferenceSample
{

    public interface IGenericReferenceValue : INamed
    {
        /// <summary>
        /// Dimension of the Value
        /// </summary>
        Units Dimension { get; }
    }

    public interface IGenericReferenceValue<T> : IGenericReferenceValue
        where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        T Value { get; }
    }



    public interface IGenericReferenceValueHandler : IGenericReferenceValue, INamedHandler
    {
        new Units Dimension { get; set; }
    }

    public interface IGenericReferenceValueHandler<T> : IGenericReferenceValueHandler, IGenericReferenceValue<T>
        where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        new T Value { get; set; }
    }


}
