using BaseClasses.MeasurementEvaluator;
using Interfaces.MeasurementEvaluator.MeasuredData;
using Interfaces.MeasurementEvaluator.Result;

namespace Interfaces.MeasurementEvaluator.Calculation
{
    public interface ICalculation
    {
        /// <summary>
        /// Performs a calculation
        /// </summary>
        /// <param name="measurementSerie">input measurement data</param>
        /// <param name="settings"></param>
        /// <returns>Calculation results</returns>
        /// <exception cref="ArgumentException">Throws when input settings are not applicable</exception>
        ICalculationResult Calculate(IMeasurementSerie measurementSerie, ICalculationSettings settings);

        /// <summary>
        /// type of the current calculation
        /// </summary>
        CalculationTypes CalculationType { get; }

    }
}
