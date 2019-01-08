using Interfaces.MeasuredData;

namespace Interfaces.Calculations
{

    public interface ICalculationResult
    {
        IMeasurementSerie Measurement { get; }
    }

    public interface ICalculationResult<T> where T : struct
    {
        T Result { get; }
    }
}
