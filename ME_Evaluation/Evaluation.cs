using DataStructures.MeasuredData;
using Frame.MessageHandler;
using Interfaces.BaseClasses;
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
            _parameters.Logger.LogMethodError("Instantiated.");
        }

        #endregion

        #region IResultProvider

        private event EventHandler<ResultEventArgs> ResultReadyEvent;


        public void SubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            _parameters.Logger.LogMethodInfo($"{method.GetMethodInfo().DeclaringType} class subscribed to {nameof(ResultReadyEvent)}");
            ResultReadyEvent += method;
        }

        public void UnSubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            _parameters.Logger.LogMethodInfo($"{method.GetMethodInfo().DeclaringType} class un-subscribed to {nameof(ResultReadyEvent)}");
            ResultReadyEvent -= method;
        }

        #endregion

        #region IInitialized

        protected override void InternalInit()
        {
            if (!_parameters.Matcher.Initiailze())
            {
                HandleInitializationFailed($"{nameof(_parameters.Matcher)} could not been initialized.");
                return;
            }

            if (!_parameters.DataCollector.Initiailze())
            {
                HandleInitializationFailed($"{nameof(_parameters.DataCollector)} could not been initialized.");
                return;
            }
            _parameters.DataCollector.SubscribeToResultReadyEvent(On_DataCollector_ResultReadyEvent);

            _processorQueue = new Queue<QueueElement>();
            _tokenSource = new CancellationTokenSource();

            _processThread = new Thread(ProcessEvaluation)
            {
                Name = "EvaluatorQueueProcessorThread",
                IsBackground = false
            };
            _processThread.Start(_tokenSource.Token);

            InitializationState = InitializationStates.Initialized;
            _parameters.MessageControl.AddMessage($"{_parameters.Name} initialized");
        }

        private void HandleInitializationFailed(string message)
        {
            _parameters.MessageControl.AddMessage(_parameters.Logger.LogError(message), MessageSeverityLevels.Error);
            InitializationState = InitializationStates.InitializationFailed;
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
            if (!(e?.Result is IDataCollectorResult collectedData))
            {
                _parameters.Logger.LogMethodError($"Received result event args is null or NOT {nameof(IDataCollectorResult)}");
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
            _parameters.Logger.LogMethodInfo($"{Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} thread started.");

            CancellationToken token;
            try
            {
                token = (CancellationToken)obj;
            }
            catch (Exception ex)
            {
                _parameters.Logger.LogMethodError($"Arrived parameter is not {nameof(CancellationToken)}. Exception: {ex}");
                return;
            }

            WaitHandle[] handles = new WaitHandle[] { _queueHandle, token.WaitHandle };

            while (true)
            {
                WaitHandle.WaitAny(handles);

                if (token.IsCancellationRequested)
                {
                    _parameters.Logger.LogMethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
                    break;
                }

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        _parameters.Logger.LogMethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
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
                        _parameters.Logger.LogMethodError("Started to process item, but it is null");
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
                _parameters.Logger.LogMethodError("Received specification is null.");
                return;
            }

            if (measurementDatas == null)
            {
                _parameters.Logger.LogMethodError("Received measurement data is null.");
                return;
            }

            _parameters.Logger.LogMethodInfo($"Started to evaluate received collectordata: Specification name: {specification.Name}");
            _parameters.Logger.LogMethodInfo($"Reference name: {referenceSample?.Name ?? "No reference received"}.");
            _parameters.Logger.LogMethodInfo($"Measurement datas: {string.Join(",", measurementDatas.Select(p => p.Name))}");

            List<IQuantityEvaluationResult> quantityResultList = new List<IQuantityEvaluationResult>();

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
                        //if (condition == null)
                        //{
                        //    _parameters.Logger.LogMethodError("Received condition is null");
                        //    continue;
                        //}

                        _parameters.MessageControl.AddMessage(_parameters.Logger.LogMethodInfo($"{quantitySpec.Quantity.Name}-{condition.Name} Evaluation started."));

                        // skip condition if not enabled:
                        if (!condition.Enabled)
                        {
                            _parameters.MessageControl.AddMessage(_parameters.Logger.LogMethodInfo($"{quantitySpec.Quantity.Name}-{condition.Name} is not enabled -> condition check skipped."));
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
                            _parameters.MessageControl.AddMessage(_parameters.Logger.LogMethodError($"{quantitySpec.Quantity.Name}-{condition.Name} No coherent measurement data was found in measurement data files. Searched names: {string.Join(",", coherentMeasurementDataNames)}"), MessageSeverityLevels.Error);
                            continue;
                        }

                        // if more result were found with the same name -> they will be linked together, unless their name is different the coherent measurement datas will be summarized into one measurement data
                        List<INumericMeasurementPoint> measPointList = new List<INumericMeasurementPoint>();
                        foreach (IMeasurementSerie serie in coherentMeasurementData)
                        {
                            measPointList.AddRange(serie.MeasuredPoints);
                        }
                        IMeasurementSerie jointCalculationInputData = new MeasurementSerie(coherentMeasurementData[0].Name, measPointList, coherentMeasurementData[0].Dimension);

                        _parameters.MessageControl.AddMessage(_parameters.Logger.LogMethodInfo($"{coherentMeasurementData.Count} measurement data(s) were created (joint together)."));

                        // find reference associated with the specification
                        string referenceName = _parameters.Matcher.GetReferenceName(condition.Name);
                        IReferenceValue referenceValue = referenceSample?.ReferenceValues?.FirstOrDefault(p => string.Equals(p.Name, referenceName));

                        // perform calculation:
                        IResult calcResult = calculation.Calculate(jointCalculationInputData, condition, referenceValue);

                        if (!calcResult.Successful)
                        {
                            _parameters.MessageControl.AddMessage(_parameters.Logger.LogMethodError($"{quantitySpec.Quantity.Name}-{condition.Name} Calculation {calculation.CalculationType} was not successful."), MessageSeverityLevels.Error);
                            continue;
                        }

                        IConditionEvaluationResult conditionEvaluationResult = condition.Evaluate(calcResult, _parameters.DateTimeProvider.GetDateTime(), referenceValue);

                        conditionResultList.Add(conditionEvaluationResult);

                        #region logging
                        if (_parameters.Logger.IsTraceEnabled)
                        {
                            _parameters.Logger.LogMethodTrace("The evaluation result:");
                            _parameters.Logger.LogMethodTrace($"   End time: {conditionEvaluationResult.CreationTime}");
                            _parameters.Logger.LogMethodTrace($"   The calculation was {(conditionEvaluationResult.Successful ? "" : "NOT")} successful.");
                            _parameters.Logger.LogMethodTrace($"   Calculation input data name {jointCalculationInputData.Name} number of measurement points: {jointCalculationInputData.MeasuredPoints.Count}");
                            _parameters.Logger.LogMethodTrace($"   ReferenceValue: {referenceValue}");
                            _parameters.Logger.LogMethodTrace($"   Condition: {condition}");
                            _parameters.Logger.LogMethodTrace($"   The result is {(conditionEvaluationResult.ConditionIsMet ? "" : "NOT")} acceptable.");
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _parameters.Logger.LogMethodError($"Exception occured: {ex}");
                    }
                }

                quantityResultList.Add(new QuantityEvaluationResult(conditionResultList, quantitySpec.Quantity));
            }

            IEvaluationResult evaluationResult = new EvaluationResult(_parameters.DateTimeProvider.GetDateTime(), true, quantityResultList, specification.ToolName, specification.Name);

            var resultreadyevent = ResultReadyEvent;
            resultreadyevent?.Invoke(this, new ResultEventArgs(evaluationResult));

            _parameters.MessageControl.AddMessage(_parameters.Logger.LogMethodError("Calculation Finished"));
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
