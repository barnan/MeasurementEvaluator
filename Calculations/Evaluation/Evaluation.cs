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

            if (!(e.Result is IDataCollectorResult collectedData))
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

            List<IQuantityEvaluationResult> quantityEvaluationList = new List<IQuantityEvaluationResult>();
            DateTime fullStartTime = _parameters.DateTimeProvider.GetDateTime();

            foreach (IQuantitySpecification quantitySpec in specification.Specifications)
            {
                List<IConditionEvaluationResult> conditionResultList = new List<IConditionEvaluationResult>(quantitySpec.Conditions.Count);

                foreach (ICondition condition in quantitySpec.Conditions)
                {
                    try
                    {
                        DateTime conditionEvaluationStartTime = _parameters.DateTimeProvider.GetDateTime();

                        if (condition == null)
                        {
                            _parameters.Logger.Info("Arrived condition is null");
                            conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                            continue;
                        }

                        // skipp condition if not enabled:
                        if (!condition.Enabled)
                        {
                            _parameters.Logger.Info($"{quantitySpec.QuantityName} {condition.Name} is not enabled -> condition check skipped.");
                            continue;
                        }

                        var calculation = _parameters.CalculationContainer.GetCalculation(condition.CalculationType);

                        // find measurement data associated with the specification name from mather:
                        List<IMeasurementSerie> coherentMeasurementData = new List<IMeasurementSerie>();
                        IEnumerable<string> coherentMeasurementDataNames = _parameters.Matcher.GetMeasDataNames(condition.Name);

                        foreach (var item in measurementDatas)
                        {
                            coherentMeasurementData.AddRange(item.Results.Where(p => coherentMeasurementDataNames.Contains(p.MeasuredQuantityName)));
                        }

                        if (coherentMeasurementData.Count == 0)
                        {
                            _parameters.Logger.LogError("No coherent measurement data was found in measurement data files");
                            conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                            continue;
                        }

                        IMeasurementSerie calculationInputData;     // the coherent measurement datas will be summarized into this
                        if (coherentMeasurementData.Count == 1)
                        {
                            calculationInputData = coherentMeasurementData[0];
                        }
                        else // if more result were found with the same name -> they will be linked together, unless their name is different
                        {
                            List<IMeasurementPoint> measPointList = new List<IMeasurementPoint>();
                            foreach (IMeasurementSerie serie in coherentMeasurementData)
                            {
                                measPointList.AddRange(serie.MeasData);
                            }

                            calculationInputData = new MeasurementSerie(coherentMeasurementData[0].MeasuredQuantityName, measPointList, coherentMeasurementData[0].Dimension);
                        }

                        // find reference associated with the specification
                        string referenceName = _parameters.Matcher.GetreferenceName(condition.Name);
                        IReferenceValue referenceValue = referenceSample.ReferenceValues.FirstOrDefault(p => string.Equals(p.Name, referenceName));

                        // perform calculation:
                        ICalculationResult calcResult = calculation.Calculate(calculationInputData);

                        if (!calcResult.SuccessfulCalculation)
                        {
                            conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                            continue;
                        }

                        bool conditionEvaluationResult = condition.Compare(calcResult);

                        IConditionEvaluationResult conditionResult = new ConditionEvaluaitonResult(
                            conditionEvaluationStartTime,
                            _parameters.DateTimeProvider.GetDateTime(),
                            calcResult.SuccessfulCalculation,
                            calculationInputData,
                            condition,
                            referenceValue,
                            conditionEvaluationResult,
                            calcResult);

                        conditionResultList.Add(conditionResult);

                        if (_parameters.Logger.IsTraceEnabled)
                        {
                            _parameters.Logger.MethodTrace("The evaluation result:");
                            _parameters.Logger.MethodTrace($"   Start time: {conditionResult.StartTime}");
                            _parameters.Logger.MethodTrace($"   End time: {conditionResult.EndTime}");
                            _parameters.Logger.MethodTrace($"   The calculation was {(conditionResult.SuccessfulCalculation ? "" : "NOT")} successful.");
                            _parameters.Logger.MethodTrace($"   Calculation input data name {calculationInputData.MeasuredQuantityName} number of measurement points: {calculationInputData.MeasData.Count}");
                            _parameters.Logger.MethodTrace($"   ReferenceValue: {referenceValue}");
                            _parameters.Logger.MethodTrace($"   Condition: {condition}");
                            _parameters.Logger.MethodTrace($"   The result is {(conditionEvaluationResult ? "" : "NOT")} acceptable.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _parameters.Logger.LogError($"Exception occured: {ex}");
                        conditionResultList.Add(CreateNOTSuccessfulConditionResult());

                    }
                }

                IQuantityEvaluationResult quantityResult = new QuantityEvaluationResult(conditionResultList);

            }

            IEvaluationResult evaluationResult = new EvaluationResult(fullStartTime, _parameters.DateTimeProvider.GetDateTime(), true, quantityEvaluationList);

            // TODO: fire event
        }


        private IConditionEvaluationResult CreateNOTSuccessfulConditionResult()
        {
            return new ConditionEvaluaitonResult(default(DateTime), default(DateTime), false, null, null, null, false, null);
        }


        #endregion

    }


}
