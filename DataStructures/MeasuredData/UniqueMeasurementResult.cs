using Interfaces.MeasuredData;
using System;

namespace DataStructures.MeasuredData
{
    public class UniqueMeasurementResult<T> : IUniqueMeasurementResult<T> where T : struct
    {
        public T Result { get; set; }

        public UniqueMeasurementResult(T result)
        {
            Result = result;
        }

    }


    public class TimedUniqueMeasurementResult<T> : UniqueMeasurementResult<T>, ITimedUniqueMeasurementResult<T> where T : struct
    {
        public DateTime MeasurementTime { get; set; }


        public TimedUniqueMeasurementResult(T result, DateTime measurementTime)
            : base(result)
        {

            Result = result;
            MeasurementTime = measurementTime;
        }

    }

}
