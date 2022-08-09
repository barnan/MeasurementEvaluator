
namespace Interfaces.MeasurementEvaluator.MeasuredData
{

    public interface IDataPoint
    {
        /// <summary>
        /// it shows whether the measured data point is valid or not
        /// </summary>
        bool IsValid { get; }

        DateTime CreationTime { get; }
    }

    public interface IDataPoint<T> : IDataPoint
    {
        /// <summary>
        /// The value of one individual measurement
        ///  </summary>
        T Value { get; }
    }
    
}
