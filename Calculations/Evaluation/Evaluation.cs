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


        public event EventHandler<ResultEventArgs> ResultReadyEvent;

        // TODO: finish queue processing


        public Evaluation(EvaluationParameters parameters)
        {
            _parameters = parameters;

            _parameters.Logger.MethodError("Instantiated.");
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
                _parameters.DataCollector.Close();

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
                if (!_parameters.DataCollector.Initiailze())
                {
                    _parameters.Logger.LogError($"{nameof(_parameters.DataCollector)} could not been initialized.");
                    return false;
                }
                _parameters.DataCollector.ResultReadyEvent += DataCollector_ResultReadyEvent;

                IsInitialized = true;

                _parameters.Logger.MethodError("Initialized.");

                return IsInitialized;
            }

        }

        #endregion

        #region private

        private void DataCollector_ResultReadyEvent(object sender, ResultEventArgs e)
        {
            if (e?.Result == null)
            {
                _parameters.Logger.MethodError("Arrived result event args is null.");
                return;
            }

            IDataCollectorResult collectedData = e.Result as IDataCollectorResult;

            if (collectedData == null)
            {
                _parameters.Logger.LogError($"Arrived result event args is not {nameof(IDataCollectorResult)}");
                return;
            }

            IToolSpecification specification = collectedData.Specification;
            IReadOnlyList<IToolMeasurementData> measurementDatas = collectedData.MeasurementData;
            IReferenceSample referenceSample = collectedData.Reference;

            if (specification == null)
            {
                _parameters.Logger.LogError("Arrived specification is null.");
                return;
            }

            if (measurementDatas == null)
            {
                _parameters.Logger.LogError("Arrived measurement data is null.");
                return;
            }





        }


        #endregion

    }


}
