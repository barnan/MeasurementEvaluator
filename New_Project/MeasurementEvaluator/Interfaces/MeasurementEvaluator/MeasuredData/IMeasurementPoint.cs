
namespace Interfaces.MeasurementEvaluator.MeasuredData
{

    public interface IMeasurementPoint
    {
        /// <summary>
        /// it shows whether the measured data point is valid or not
        /// </summary>
        bool IsValid { get; }
    }

    public interface IMeasurementPoint<T> : IMeasurementPoint
    {
        /// <summary>
        /// The value of one individual measurement
        ///  </summary>
        T Value { get; }
    }
    
}
