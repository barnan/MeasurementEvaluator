using System;

namespace Interfaces.MeasuredData
{
    public interface IMeasurementPoint
    {

        /// <summary>
        /// it shows whether the measured data point is valid or not
        /// </summary>
        bool Valid { get; }

        double Value { get; }
    }



    //public interface IMeasurementPoint<out T> : IMeasurementPoint where T : struct
    //{
    //    /// <summary>
    //    /// result of one individual the measurement session
    //    /// </summary>
    //    T Result { get; }
    //}


    public interface ITimedMeasurementPoint : IMeasurementPoint
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; }
    }


}
