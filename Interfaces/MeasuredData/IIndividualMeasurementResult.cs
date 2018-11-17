using System;

namespace Interfaces.MeasuredData
{
    public interface IIndividualMeasurementResult
    {
    }

    interface IIndividualMeasurementResult<T> : IIndividualMeasurementResult where T : struct
    {
        /// <summary>
        /// result of one individual the measurement session
        /// </summary>
        T Result { get; set; }
    }

    interface ITimedIndividualMeasurementResult<T> : IIndividualMeasurementResult<T> where T : struct
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; set; }
    }


}
