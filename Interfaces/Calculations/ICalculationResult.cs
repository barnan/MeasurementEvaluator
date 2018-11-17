using Interfaces.MeasuredData;

namespace Interfaces.Calculations
{

    public interface ICalculationResult
    {
        IMeasurementSerieData MeasurementData { get; }
    }

    public interface ICalculationResult<T> where T : struct
    {
        T Result { get; }
    }
}
