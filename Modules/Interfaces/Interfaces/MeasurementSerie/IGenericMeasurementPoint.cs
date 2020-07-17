using System;

namespace Interfaces.MeasurementSerie
{

    public interface IValidableMeasurementPoint
    {
        /// <summary>
        /// it shows whether the measured data point is valid or not
        /// </summary>
        bool Valid { get; }
    }


    public interface IGenericMeasurementPoint<T> : IValidableMeasurementPoint
        where T : struct
    {
        /// <summary>
        /// The value of one individual measurement
        /// </summary>
        T Value { get; }
    }


    public interface ITimedMeasurementPoint
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; }
    }

}
