using System;

namespace Interfaces.MeasuredData
{
    public interface IUniqueMeasurementResult
    {

        /// <summary>
        /// it shows whether the measured data point is valid or not
        /// </summary>
        bool valid { get; }
    }



    public interface IUniqueMeasurementResult<T> : IUniqueMeasurementResult where T : struct
    {
        /// <summary>
        /// result of one individual the measurement session
        /// </summary>
        T Result { get; }
    }


    public interface ITimedUniqueMeasurementResult<T> : IUniqueMeasurementResult<T> where T : struct
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; }
    }


}
