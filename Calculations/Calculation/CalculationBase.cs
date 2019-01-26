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
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initilaized yet.");
                return null;
            }

            if (measurementSerieData?.MeasData == null)
            {
                _parameters.Logger.LogError("Arrived measdata is null.");

                return null;
            }

            return InternalCalculation(measurementSerieData, settings);
        }

        #endregion


        #region IInitialized

        public bool IsInitialized { get; private set; }

        public event EventHandler<EventArgs> Initialized;
        public event EventHandler<EventArgs> Closed;

        public void Close()
        {
            if (!IsInitialized)
            {
                return;
            }

            lock (_lockObj)
            {
                if (!IsInitialized)
                {
                    return;
                }



                IsInitialized = false;

                _parameters.Logger.MethodInfo("Closed.");
            }
        }

        public bool Initiailze()
        {
            if (IsInitialized)
            {
                return true;
            }

            lock (_lockObj)
            {
                if (IsInitialized)
                {
                    return true;
                }

                IsInitialized = true;

                _parameters.Logger.MethodInfo("Initialized.");

                return IsInitialized;
            }

        }

        #endregion


        public CalculationBase(CalculationParameters parameters)
        {
            _parameters = parameters;

            _parameters.Logger.MethodError("Instantiated.");
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
