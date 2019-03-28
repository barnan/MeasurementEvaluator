using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.Result;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculations.Calculation
{
    internal abstract class CalculationBase : ICalculation
    {

        private readonly object _lockObj = new object();
        protected readonly CalculationParameters _parameters;


        #region ICalculation

        public abstract CalculationTypes CalculationType { get; }

        public ICalculationResult Calculate(IMeasurementSerie measurementSerieData, ICalculationSettings settings = null)
        {
            if (measurementSerieData?.MeasData == null)
            {
                _parameters.Logger.LogError("Received measdata is null.");

                return null;
            }

            return InternalCalculation(measurementSerieData, settings);
        }

        #endregion



        internal CalculationBase(CalculationParameters parameters)
        {
            _parameters = parameters;
        }


        protected abstract ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICalculationSettings settings);


        protected virtual List<double> GetValidElementList(IMeasurementSerie measurementSerieData)
        {
            return measurementSerieData.MeasData.Where(p => p.Valid).Select(p => p.Value).ToList();
        }


        protected virtual double GetAverage(List<double> inputData)
        {
            return inputData.Average();
        }


        protected virtual double GetStandardDeviation(List<double> inputData)
        {
            double average = GetAverage(inputData);

            double sumOfDerivation = 0;

            foreach (double value in inputData)
            {
                sumOfDerivation += value * value;
            }
            double sumOfDerivationAverage = sumOfDerivation / (inputData.Count - 1);

            return Math.Sqrt(sumOfDerivationAverage - average * average);
        }

    }
}
