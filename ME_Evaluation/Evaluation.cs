using DataStructures.MeasuredData;
using Interfaces;
using Interfaces.Calculation;
using Interfaces.Evaluation;
using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace MeasurementEvaluator.ME_Evaluation
{
    internal class Evaluation : InitializableBase, IEvaluation
    {
        private readonly object _queueLockObj = new object();
        private readonly EvaluationParameters _parameters;
        private readonly AutoResetEvent _queueHandle = new AutoResetEvent(false);
        private Queue<QueueElement> _processorQueue;
        private CancellationTokenSource _tokenSource;
        private Thread _processThread;
        private const int _PROCESS_THREAD_JOINT_TIMEOUT_MS = 500;


        #region ctor

        public Evaluation(EvaluationParameters parameters)
            : base(parameters.Logger)
        {
            _parameters = parameters;
            _parameters.Logger.MethodError("Instantiated.");
        }

        #endregion

        #region IResultProvider

        private event EventHandler<ResultEventArgs> ResultReadyEvent;


        public void SubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            _parameters.Logger.MethodInfo($"{method.GetMethodInfo().DeclaringType} class subscribed to {nameof(ResultReadyEvent)}");
            ResultReadyEvent += method;
        }

        public void UnSubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            _parameters.Logger.MethodInfo($"{method.GetMethodInfo().DeclaringType} class un-subscribed to {nameof(ResultReadyEvent)}");
            ResultReadyEvent -= method;
        }

        #endregion

        #region IInitialized

        protected override void InternalInit()
        {
            if (!_parameters.Matcher.Initiailze())
            {
                _parameters.Logger.MethodError($"{nameof(_parameters.Matcher)} could not been initialized.");
                InitializationState = InitializationStates.InitializationFailed;
                return;
            }

            if (!_parameters.DataCollector.Initiailze())
            {
                _parameters.Logger.MethodError($"{nameof(_parameters.DataCollector)} could not been initialized.");
                InitializationState = InitializationStates.InitializationFailed;
                return;
            }
            _parameters.DataCollector.SubscribeToResultReadyEvent(On_DataCollector_ResultReadyEvent);

            _processorQueue = new Queue<QueueElement>();
            _tokenSource = new CancellationTokenSource();

            _processThread = new Thread(ProcessEvaluation)
            {
                Name = "EvaluatorQueueProcesorThread",
                IsBackground = false
            };
            _processThread.Start(_tokenSource.Token);

            InitializationState = InitializationStates.Initialized;
        }

        protected override void InternalClose()
        {
            _parameters.DataCollector.UnSubscribeToResultReadyEvent(On_DataCollector_ResultReadyEvent);

            _tokenSource.Cancel();
            _processorQueue.Clear();

            _parameters.DataCollector.Close();
            _processThread.Join(_PROCESS_THREAD_JOINT_TIMEOUT_MS);

            InitializationState = InitializationStates.NotInitialized;
        }

        #endregion

        #region private

        private void On_DataCollector_ResultReadyEvent(object sender, ResultEventArgs e)
        {
            if (e?.Result == null)
            {
                _parameters.Logger.MethodError("Received result event args is null.");
                return;
            }

            if (!(e.Result is IDataCollectorResult collectedData))
            {
                _parameters.Logger.MethodError($"Received result event args is not {nameof(IDataCollectorResult)}");
                return;
            }

            lock (_queueLockObj)
            {
                _processorQueue.Enqueue(new QueueElement(collectedData));
                _queueHandle.Set();
            }
        }


        private void ProcessEvaluation(object obj)
        {
            _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} thread started.");

            CancellationToken token;
            try
            {
                token = (CancellationToken)obj;
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Arrived parameter is not {nameof(CancellationToken)}. Exception: {ex}");
                return;
            }

            WaitHandle[] handles = new WaitHandle[] { _queueHandle, token.WaitHandle };

            while (true)
            {
                WaitHandle.WaitAny(handles);

                if (token.IsCancellationRequested)
                {
                    _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
                    break;
                }

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
                        break;
                    }

                    QueueElement item = null;
                    lock (_queueLockObj)
                    {
                        if (_processorQueue.Count > 0)
                        {
                            _parameters.Logger.LogTrace("New queue element arrived!");
                            item = _processorQueue.Dequeue();
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (item == null)
                    {
                        _parameters.Logger.MethodError("Started to process item, but it is null");
                        continue;
                    }

                    Evaluate(item);
                }
            }
        }


        private void Evaluate(QueueElement element)
        {
            IDataCollectorResult collectedData = element.DataCollectorResult;

            IToolSpecification specification = collectedData.Specification;
            IReadOnlyList<IToolMeasurementData> measurementDatas = collectedData.MeasurementData;
            IReferenceSample referenceSample = collectedData.Reference;

            if (specification == null)
            {
                _parameters.Logger.MethodError("Received specification is null.");
                return;
            }

            if (measurementDatas == null)
            {
                _parameters.Logger.MethodError("Received measurement data is null.");
                return;
            }

            _parameters.Logger.MethodInfo($"Started to evaluate received collectordata: Specification name: {specification.Name}");
            _parameters.Logger.MethodInfo($"Reference name: {referenceSample?.Name ?? "No reference received"}.");
            _parameters.Logger.MethodInfo("Measurement datas: ");
            foreach (IToolMeasurementData measurementData in measurementDatas)
            {
                _parameters.Logger.MethodInfo(measurementData.Name);
            }

            List<IQuantityEvaluationResult> quantityEvaluationList = new List<IQuantityEvaluationResult>();

            // go through all quantity specifications:
            foreach (IQuantitySpecification quantitySpec in specification.QuantitySpecifications)
            {
                List<IConditionEvaluationResult> conditionResultList = new List<IConditionEvaluationResult>(quantitySpec.Conditions.Count);

                // go through all conditions in one quantity specification
                foreach (ICondition condition in quantitySpec.Conditions)
                {
                    try
                    {
                        // skip condition if condition is null:
                        if (condition == null)
                        {
                            _parameters.Logger.MethodInfo("Received condition is null");
                            conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                            continue;
                        }

                        // skip condition if not enabled:
                        if (!condition.Enabled)
                        {
                            _parameters.Logger.MethodInfo($"{quantitySpec.Quantity.Name} {condition.Name} is not enabled -> condition check skipped.");
                            conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                            continue;
                        }

                        // get the calculation:
                        ICalculation calculation = _parameters.CalculationContainer.GetCalculation(condition.CalculationType);

                        // find measurement data associated with the condition name from the matcher:
                        IEnumerable<string> coherentMeasurementDataNames = _parameters.Matcher.GetMeasDataNames(condition.Name);
                        List<IMeasurementSerie> coherentMeasurementData = new List<IMeasurementSerie>();
                        foreach (var item in measurementDatas)
                        {
                            coherentMeasurementData.AddRange(item.Results.Where(p => coherentMeasurementDataNames.Contains(p.Name)));
                        }

                        if (coherentMeasurementData.Count == 0)
                        {
                            _parameters.Logger.MethodError("No coherent measurement data was found in measurement data files");
                            conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                            continue;
                        }

                        // if more result were found with the same name -> they will be linked together, unless their name is different
                        // the coherent measurement datas will be summarized into one measurement data
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
                                measPointList.AddRange(serie.MeasuredPoints);
                            }
                            calculationInputData = new MeasurementSerie(coherentMeasurementData[0].Name, measPointList, coherentMeasurementData[0].Dimension);
                        }

                        // find reference associated with the specification
                        string referenceName = _parameters.Matcher.GetReferenceName(condition.Name);
                        IReferenceValue referenceValue = referenceSample?.ReferenceValues?.FirstOrDefault(p => string.Equals(p.Name, referenceName));

                        // perform calculation:
                        IResult calcResult = calculation.Calculate(calculationInputData, condition, referenceValue);

                        if (!calcResult.Successful)
                        {
                            conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                            continue;
                        }

                        bool conditionEvaluationResult = condition.Compare(calcResult);

                        IConditionEvaluationResult conditionResult = new ConditionEvaluaitonResult(
                            _parameters.DateTimeProvider.GetDateTime(),
                            calcResult.Successful,
                            calculationInputData,
                            condition,
                            referenceValue,
                            conditionEvaluationResult,
                            calcResult);

                        conditionResultList.Add(conditionResult);


                        #region logging
                        if (_parameters.Logger.IsTraceEnabled)
                        {
                            _parameters.Logger.MethodTrace("The evaluation result:");
                            _parameters.Logger.MethodTrace($"   End time: {conditionResult.CreationTime}");
                            _parameters.Logger.MethodTrace($"   The calculation was {(conditionResult.Successful ? "" : "NOT")} successful.");
                            _parameters.Logger.MethodTrace($"   Calculation input data name {calculationInputData.Name} number of measurement points: {calculationInputData.MeasuredPoints.Count}");
                            _parameters.Logger.MethodTrace($"   ReferenceValue: {referenceValue}");
                            _parameters.Logger.MethodTrace($"   Condition: {condition}");
                            _parameters.Logger.MethodTrace($"   The result is {(conditionEvaluationResult ? "" : "NOT")} acceptable.");
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _parameters.Logger.MethodError($"Exception occured: {ex}");
                        conditionResultList.Add(CreateNOTSuccessfulConditionResult());
                    }
                }

                quantityEvaluationList.Add(new QuantityEvaluationResult(conditionResultList));
            }
            IEvaluationResult evaluationResult = new EvaluationResult(_parameters.DateTimeProvider.GetDateTime(), true, quantityEvaluationList);

            var resultreadyevent = ResultReadyEvent;
            resultreadyevent?.Invoke(this, new ResultEventArgs(evaluationResult));
        }



        private IConditionEvaluationResult CreateNOTSuccessfulConditionResult()
        {
            return new ConditionEvaluaitonResult(default(DateTime), false, null, null, null, false, null);
        }

        #endregion





        private class QueueElement
        {
            internal IDataCollectorResult DataCollectorResult { get; }

            internal QueueElement(IDataCollectorResult result)
            {
                DataCollectorResult = result;
            }
        }
    }
}
