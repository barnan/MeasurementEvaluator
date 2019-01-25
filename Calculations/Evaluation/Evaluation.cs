using Interfaces;
using Interfaces.Evaluation;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;

namespace Calculations.Evaluation
{
    internal class Evaluation : IEvaluation
    {
        private readonly object _lockObj = new object();
        private readonly EvaluationParameters _parameters;
        private Queue<QueueElement> _processQueue;

        public event EventHandler<ResultEventArgs> ResultReadyEvent;

        // TODO: finish queue processing


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

                _parameters.DataCollector.ResultReadyEvent -= DataCollector_ResultReadyEvent;

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

                _parameters.DataCollector.ResultReadyEvent += DataCollector_ResultReadyEvent;

                IsInitialized = true;

                _parameters.Logger.MethodError($"Instantiated.");

                return IsInitialized;
            }

        }

        #endregion


        private void DataCollector_ResultReadyEvent(object sender, ResultEventArgs e)
        {
            if (e?.Result == null)
            {
                _parameters.Logger.MethodError("Arrived result event args is null.");
                return;
            }

            ICalculationResult calculationResult = e.Result as ICalculationResult;

            //if (calculationResult?.CalculationResults == null || calculationResult.CalculationResults.Count == 0)
            //{
            //    _parameters.Logger.LogError($"Arrived result event args is not {nameof(ICalculationResult)} or its {nameof(calculationResult.CalculationResults)} field is empty");
            //    return;
            //}

            //foreach (IConditionCalculationResult<double> item in calculationResult.CalculationResults)
            //{
            //    if (!item.Condition.Enabled)
            //    {
            //        _parameters.Logger.LogTrace($"{item.Condition.Name} is not enabled condition.");
            //        continue;
            //    }



            //}

        }

    }



    internal class QueueElement
    {
        IMeasurementSerie MeasurementSerieData { get; }
        ICondition Condition { get; }
        IReferenceValue ReferenceValue { get; }
    }


}
