using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.Result;

namespace Interfaces.Calculation
{

    public interface ICalculation : IInitializable
    {
        /// <summary>
        /// Performs calculation
        /// </summary>
        /// <param name="measurementSerieData">input measurement data</param>
        /// <param name="settings">settings for the calculation</param>
        /// <returns></returns>
        ICalculationResult Calculate(IMeasurementSerie measurementSerieData, ICalculationSettings settings = null);

        /// <summary>
        /// Gives the type of the current calculation
        /// </summary>
        CalculationTypes CalculationType { get; }
    }

}
