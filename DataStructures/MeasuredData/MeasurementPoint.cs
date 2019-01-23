using Interfaces.MeasuredData;
using System;

namespace DataStructures.MeasuredData
{
    public class MeasurementPoint : IMeasurementPoint
    {
        public double Value { get; }

        public bool Valid { get; }


        public MeasurementPoint(double value, bool valid)
        {
            Value = value;
            Valid = valid;
        }

    }


    public class TimedMeasurementPoint : MeasurementPoint, ITimedMeasurementPoint
    {
        public DateTime MeasurementTime { get; }


        public TimedMeasurementPoint(double result, bool valid, DateTime measurementTime)
            : base(result, valid)
        {
            MeasurementTime = measurementTime;
        }

    }

}
