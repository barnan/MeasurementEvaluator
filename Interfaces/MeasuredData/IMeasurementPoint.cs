using System;

namespace Interfaces.MeasuredData
{
    public interface IMeasurementPoint : IGenericMeasurementPoint<double>
    {
    }


    public interface ITimedMeasurementPoint : IMeasurementPoint
    {
        /// <summary>
        /// date and time when the measurement was taken
        /// </summary>
        DateTime MeasurementTime { get; }
    }


}
