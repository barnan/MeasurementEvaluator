using System;

namespace Interfaces.MeasuredData
{
    public interface INumericMeasurementPoint : IGenericMeasurementPoint<double>
    {
    }


    public interface ITimedNumericMeasurementPoint : INumericMeasurementPoint
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; }
    }


}
