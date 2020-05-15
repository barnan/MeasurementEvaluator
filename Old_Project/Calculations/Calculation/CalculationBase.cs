using Interfaces.BaseClasses;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
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

        public abstract CalculationType CalculationType { get; }

        public ICalculationResult Calculate(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            if (measurementSerieData?.MeasuredPoints == null)
            {
                _parameters.Logger.LogError("Received measdata is null.");
                return null;
            }

            try
            {
                if (condition.CalculationTypeHandler.CalculationType != CalculationType)
                {
                    throw new ArgumentException($"The current calculation (type: {CalculationType}) can not run with the received condition {condition.CalculationTypeHandler}");
                }

                return InternalCalculation(measurementSerieData, condition, referenceValue);
            }
            catch (Exception ex)
            {
                _parameters.Logger.LogError($"Exception occured:{ex.Message}");
                return null;
            }
        }

        #endregion



        internal CalculationBase(CalculationParameters parameters)
        {
            _parameters = parameters;
        }


        protected abstract ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue);


        protected virtual List<double> GetValidElementList(IMeasurementSerie measurementSerieData)
        {
            return measurementSerieData.MeasuredPoints.Where(p => p.Valid).Select(p => p.Value).ToList();
        }


        protected virtual double GetAverage(List<double> inputData)
        {
            return inputData.Average();
        }


        protected virtual double GetStandardDeviation(List<double> inputData, double average)
        {
            double sumOfDerivation = 0;

            foreach (double value in inputData)
            {
                sumOfDerivation += (value - average) * (value - average);
            }
            double sumOfDerivationAverage = sumOfDerivation / (inputData.Count - 1);

            return Math.Sqrt(sumOfDerivationAverage);
        }

    }
}
