namespace Interfaces.MeasuredData
{
    public interface IGenericMeasurementPoint<T>
    {
        /// <summary>
        /// it shows whether the measured data point is valid or not
        /// </summary>
        bool Valid { get; }

        /// <summary>
        /// The value of one individual measurement
        ///  </summary>
        T Value { get; }
    }
}
