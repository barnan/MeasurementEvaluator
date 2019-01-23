using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.Result;
using Miscellaneous;
using System;

namespace Calculations.Calculation
{
    internal abstract class CalculationBase : ICalculation
    {

        private readonly object _lockObj = new object();
        private readonly CalculationParameters _parameters;


        #region ICalculation

        public abstract CalculationTypes CalculationType { get; }

        public ICalculationResult Calculate(IMeasurementSerie measurementSerieData)
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initilaized yet.");
                return null;
            }

            return InternalCalculation(measurementSerieData);
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

                _parameters.Logger.MethodError("Closed.");
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

                _parameters.Logger.MethodError("Initialized.");

                return IsInitialized;
            }

        }

        #endregion


        public CalculationBase(CalculationParameters parameters)
        {
            _parameters = parameters;

            _parameters.Logger.MethodError("Instantiated.");
        }


        protected abstract ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData);

    }
}
