using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.Result;

namespace Interfaces.Calculation
{

    public interface ICalculation : IInitializable
    {

        ICalculationResult Calculate(IMeasurementSerie measurementSerieData, ICalculationSettings settings = null);

        CalculationTypes CalculationType { get; }

    }

}
