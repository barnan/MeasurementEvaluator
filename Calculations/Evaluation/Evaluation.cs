using DataStructures.MeasuredData;
using Interfaces;
using Interfaces.Evaluation;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;

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

            foreach (IQuantitySpecification quantitySpec in specification.Specifications)
            {
                foreach (ICondition condition in quantitySpec.Conditions)
                {
                    if (!(condition is ISimpleCondition simpleCondition))
                    {
                        _parameters.Logger.Info($"{quantitySpec.QuantityName} {condition.Name} is not simplecondition");
                        continue;
                    }

                    if (!simpleCondition.Enabled)
                    {
                        _parameters.Logger.Info($"{quantitySpec.QuantityName} {simpleCondition.Name} is not enabled -> skipped.");
                        continue;
                    }

                    var calculation = _parameters.CalculationContainer.GetCalculation(simpleCondition.CalculationType);

                    // find measurement data associated with the specification name from mathing
                    List<IMeasurementSerie> coherentMeasurementData = new List<IMeasurementSerie>();
                    IEnumerable<string> coherentMeasurementDataNames = _parameters.Matcher.GetMeasDataNames(simpleCondition.Name);

                    foreach (var item in measurementDatas)
                    {
                        coherentMeasurementData.AddRange(item.Results.Where(p => coherentMeasurementDataNames.Contains(p.MeasuredQuantityName)));
                    }

                    if (coherentMeasurementData.Count == 0)
                    {
                        _parameters.Logger.LogError("No coherent measurement data was found in ");
                        continue;
                    }

                    IMeasurementSerie calculationInputData;
                    if (coherentMeasurementData.Count == 1)
                    {
                        calculationInputData = coherentMeasurementData[0];
                    }
                    else  // if more result were found with the same name -> they will be linked together, unless their name is different
                    {
                        List<IMeasurementPoint> measPointList = new List<IMeasurementPoint>();
                        foreach (IMeasurementSerie serie in coherentMeasurementData)
                        {
                            measPointList.AddRange(serie.MeasData);
                        }
                        calculationInputData = new MeasurementSerie(coherentMeasurementData[0].MeasuredQuantityName, measPointList, coherentMeasurementData[0].Dimension);
                    }

                    // find reference associated with the specification
                    string referenceName = _parameters.Matcher.GetreferenceName(simpleCondition.Name);
                    IReferenceValue referenceValue = referenceSample.ReferenceValues.FirstOrDefault(p => string.Equals(p.Name, referenceName));

                    // perform calculation:
                    DateTime startTime = _parameters.DateTimeProvider.GetDateTime();
                    ICalculationResult calcResult = calculation.Calculate(calculationInputData);
                    IConditionEvaluationResult conditionResult = new ConditionEvaluaitonResult(
                        startTime,
                        _parameters.DateTimeProvider.GetDateTime(),
                        calcResult.SuccessfulCalculation,
                        calculationInputData,
                        simpleCondition,
                        referenceValue,
                        simpleCondition.Compare(calcResult),
                        calcResult);


                    // TODO : condition check

                }
            }


        }


        #endregion

    }


}
