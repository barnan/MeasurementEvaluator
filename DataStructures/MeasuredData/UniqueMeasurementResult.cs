using Interfaces.MeasuredData;
using System;

namespace DataStructures.MeasuredData
{
    public class UniqueMeasurementResult<T> : IUniqueMeasurementResult<T> where T : struct
    {
        public T Result { get; }

        public bool Valid { get; }


        public UniqueMeasurementResult(T result, bool valid)
        {
            Result = result;
            Valid = valid;
        }

    }


    public class TimedUniqueMeasurementResult<T> : UniqueMeasurementResult<T>, ITimedUniqueMeasurementResult<T> where T : struct
    {
        public DateTime MeasurementTime { get; }


        public TimedUniqueMeasurementResult(T result, bool valid, DateTime measurementTime)
            : base(result, valid)
        {
            MeasurementTime = measurementTime;
        }

    }

}
