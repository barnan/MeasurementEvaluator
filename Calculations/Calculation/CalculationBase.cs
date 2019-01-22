using Interfaces;
using Interfaces.Calculation;
using Miscellaneous;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    internal abstract class CalculationBase : ICalculation
    {
        private readonly object _lockObj = new object();
        private readonly CalculationParameters _parameters;

        public IReadOnlyList<CalculationTypes> AvailableCalculationTypes { get; }

        public event EventHandler<ResultEventArgs> ResultReadyEvent;



        public CalculationBase(CalculationParameters parameters)
        {
            _parameters = parameters;

            _parameters.Logger.MethodError($"Instantiated.");
        }


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

                _parameters.Logger.MethodError($"Closed.");

                return;
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

                _parameters.Logger.MethodError($"Instantiated.");

                return IsInitialized;
            }

        }

        #endregion

    }
}
