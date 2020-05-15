using Interfaces.MeasuredData;
using System;

namespace DataStructures.MeasuredData
{
    public class NumericMeasurementPoint : INumericMeasurementPoint
    {
        public double Value { get; }

        public bool Valid { get; }


        public NumericMeasurementPoint(double value, bool valid)
        {
            Value = value;
            Valid = valid;
        }

    }


    public class TimedNumericMeasurementPoint : NumericMeasurementPoint, ITimedNumericMeasurementPoint
    {
        public DateTime MeasurementTime { get; }


        public TimedNumericMeasurementPoint(double result, bool valid, DateTime measurementTime)
            : base(result, valid)
        {
            MeasurementTime = measurementTime;
        }

    }

}
