using Interfaces.MeasuredData;

namespace Interfaces.Calculations
{
    public interface ICalculation
    {
        ICalculationResult DoCalculation(IMeasurementSerieData inputData);
    }




}
