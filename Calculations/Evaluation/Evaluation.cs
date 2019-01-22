using Interfaces;
using Interfaces.Evaluation;
using Interfaces.Result;
using Miscellaneous;
using System;

namespace Calculations.Evaluation
{
    class Evaluation : IEvaluation
    {
        private readonly object _lockObj = new object();
        private readonly EvaluationParameters _parameters;


        public event EventHandler<ResultEventArgs> ResultReadyEvent;



        public Evaluation(EvaluationParameters parameters)
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

                _parameters.Calculation.ResultReadyEvent -= Calculation_ResultReadyEvent;

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

                _parameters.Calculation.ResultReadyEvent += Calculation_ResultReadyEvent;

                IsInitialized = true;

                _parameters.Logger.MethodError($"Instantiated.");

                return IsInitialized;
            }

        }

        #endregion


        private void Calculation_ResultReadyEvent(object sender, ResultEventArgs e)
        {
            if (e?.Result == null)
            {
                _parameters.Logger.MethodError("Arrived result event args is null.");
                return;
            }

            ICalculationResult calculationResult = e.Result as ICalculationResult;

            if (calculationResult?.CalculationResults == null || calculationResult.CalculationResults.Count == 0)
            {
                _parameters.Logger.LogError($"Arrived result event args is not {nameof(ICalculationResult)} or its {nameof(calculationResult.CalculationResults)} field is empty");
                return;
            }

            foreach (IConditionCalculationResult<double> item in calculationResult.CalculationResults)
            {
                if (!item.Condition.Enabled)
                {
                    _parameters.Logger.LogTrace($"{item.Condition.Name} is not enabled condition.");
                    continue;
                }



            }

        }

    }
}
