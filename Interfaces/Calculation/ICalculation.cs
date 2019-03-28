using Interfaces.MeasuredData;
using Interfaces.Result;
using System;


namespace Interfaces.Calculation
{

    public interface ICalculation
    {
        /// <summary>
        /// Performs calculation
        /// </summary>
        /// <param name="measurementSerieData">input measurement data</param>
        /// <param name="settings">settings for the calculation</param>
        /// <returns>Calculation results</returns>
        /// <exception cref="ArgumentException">Throws when input settings are not applicable</exception>
        ICalculationResult Calculate(IMeasurementSerie measurementSerieData, ICalculationSettings settings = null);

        /// <summary>
        /// Gives the type of the current calculation
        /// </summary>
        CalculationTypes CalculationType { get; }
    }

    public interface IAverageCalculation : ICalculation
    {
    }


    public interface ICpkCalculation : ICalculation
    {
    }


    public interface ICpCalculation : ICalculation
    {
    }


    public interface IStdCalculation : ICalculation
    {
    }

}
