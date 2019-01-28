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

            IEvaluationResult evaluationresultResult = new EvaluationRe

            foreach (IQuantitySpecification quantitySpec in specification.Specifications)
            {
                foreach (ICondition condition in quantitySpec.Conditions)
                {
                    var calculation = _parameters.CalculationContainer.GetCalculation(condition.CalculationType);
                    string quantityName = quantitySpec.QuantityName;


                    List<IMeasurementSerie> coherentMeasurementData = new List<IMeasurementSerie>();
                    foreach (var item in measurementDatas)
                    {
                        coherentMeasurementData.AddRange(item.Results.Where(p =>
                        {
                            string specificationNameOfMeasData = _parameters.Matcher.GetSpecification(p.MeasuredQuantityName);
                            if (specificationNameOfMeasData == null)
                            {
                                return false;
                            }

                            return quantityName == specificationNameOfMeasData;
                        }));
                    }

                    if (coherentMeasurementData.Count == 0)
                    {
                        _parameters.Logger.LogError("No coherent measurement data was found!");
                        return;
                    }

                    IMeasurementSerie calculationInputData;
                    if (coherentMeasurementData.Count == 1)
                    {
                        calculationInputData = coherentMeasurementData[0];
                    }
                    else
                    {
                        List<IMeasurementPoint> measPointList = new List<IMeasurementPoint>();
                        foreach (IMeasurementSerie serie in coherentMeasurementData)
                        {
                            measPointList.AddRange(serie.MeasData);
                        }
                        calculationInputData = new MeasurementSerie(coherentMeasurementData[0].MeasuredQuantityName, measPointList, coherentMeasurementData[0].Dimension);
                    }

                    DateTime startTime = _parameters.DateTimeProvider.GetDateTime();
                    ICalculationResult calcResult = calculation.Calculate(calculationInputData);
                    IConditionEvaluationResult conditionResult = new ConditionEvaluaitonResult(startTime, _parameters.DateTimeProvider.GetDateTime(), calcResult.SuccessfulCalculation, calculationInputData, condition,  )

                }
            }




            foreach (IToolMeasurementData data in measurementDatas)
            {
                foreach (IMeasurementSerie measSerie in data.Results)
                {
                    measSerie.MeasData
                }

            }


        }


        #endregion

    }


}
