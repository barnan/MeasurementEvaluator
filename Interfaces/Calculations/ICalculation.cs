using Interfaces.MeasuredData;

namespace Interfaces.Calculations
{
    interface ICalculation
    {
        ICalculationResult DoCalculation(IMeasurementSeriesData inputData);
    }




}
