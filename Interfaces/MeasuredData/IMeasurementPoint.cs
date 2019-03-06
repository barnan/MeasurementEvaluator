using System;

namespace Interfaces.MeasuredData
{
    public interface IMeasurementPoint
    {
        /// <summary>
        /// it shows whether the measured data point is valid or not
        /// </summary>
        bool Valid { get; }

        /// <summary>
        /// The value of one individual measurement
        ///  </summary>
        double Value { get; }
    }


    public interface ITimedMeasurementPoint : IMeasurementPoint
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; }
    }


}
