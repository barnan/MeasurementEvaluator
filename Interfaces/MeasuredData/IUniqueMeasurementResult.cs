using System;

namespace Interfaces.MeasuredData
{
    public interface IUniqueMeasurementResult
    {
    }

    public interface IUniqueMeasurementResult<T> : IUniqueMeasurementResult where T : struct
    {
        /// <summary>
        /// result of one individual the measurement session
        /// </summary>
        T Result { get; set; }
    }


    public interface ITimedUniqueMeasurementResult<T> : IUniqueMeasurementResult<T> where T : struct
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; set; }
    }


}
