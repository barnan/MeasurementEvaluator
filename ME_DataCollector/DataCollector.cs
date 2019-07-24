using Frame.PluginLoader;
using Interfaces.BaseClasses;
using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace MeasurementEvaluator.ME_DataCollector
{
    internal class DataCollector : InitializableBase, IDataCollector
    {

        private readonly DataCollectorParameters _parameters;
        private readonly object _queueLockObj = new object();

        private Queue<QueueElement> _processorQueue;
        private AutoResetEvent _processQueueResetEvent = new AutoResetEvent(false);
        private CancellationTokenSource _tokenSource;
        private Thread _processThread;
        private const int _PROCESS_THREAD_JOINT_TIMEOUT_MS = 500;


        #region ctor

        public DataCollector(DataCollectorParameters parameters)
            : base(parameters.Logger)
        {
            _parameters = parameters;

            _parameters.SpecificationRepository.SetFolder(PluginLoader.SpecificationFolder);
            _parameters.ReferenceRepository.SetFolder(PluginLoader.ReferenceFolder);
            _parameters.MeasurementDataRepository.SetFolder(PluginLoader.MeasurementDataFolder);

            _parameters.Logger.MethodInfo("Instantiated.");
        }

        #endregion

        #region IResultProvider

        private event EventHandler<ResultEventArgs> ResultReadyEvent;

        public void SubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            ResultReadyEvent += method;

            _parameters.Logger.MethodInfo($"{method.GetMethodInfo().DeclaringType} class subscribed to {nameof(ResultReadyEvent)}");
        }

        public void UnSubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            ResultReadyEvent -= method;

            _parameters.Logger.MethodInfo($"{method.GetMethodInfo().DeclaringType} class un-subscribed to {nameof(ResultReadyEvent)}");
        }


        #endregion

        #region IInitializable


        protected override void InternalInit()
        {
            if (!_parameters.SpecificationRepository.Initiailze())
            {
                _parameters.Logger.LogError($"{_parameters.SpecificationRepository} could not been initialized.");
                InitializationState = InitializationStates.InitializationFailed;
                return;
            }

            if (!_parameters.ReferenceRepository.Initiailze())
            {
                _parameters.Logger.LogError($"{_parameters.ReferenceRepository} could not been initialized.");
                InitializationState = InitializationStates.InitializationFailed;
                return;
            }

            if (!_parameters.MeasurementDataRepository.Initiailze())
            {
                _parameters.Logger.LogError($"{_parameters.MeasurementDataRepository} could not been initialized.");
                InitializationState = InitializationStates.InitializationFailed;
                return;
            }

            _processorQueue = new Queue<QueueElement>();
            _tokenSource = new CancellationTokenSource();

            // start queue procesor thread:
            _processThread = new Thread(ProcessQueueElements)
            {
                IsBackground = false,
                Name = "DataCollectorQueueElementProcessor"
            };
            _processThread.Start(_tokenSource.Token);

            InitializationState = InitializationStates.Initialized;

            _parameters.MessageControl.AddMessage($"{_parameters.Name} initialized.");
        }

        protected override void InternalClose()
        {
            _tokenSource.Cancel();
            _processorQueue.Clear();

            _parameters.SpecificationRepository.Close();
            _parameters.ReferenceRepository.Close();
            _parameters.MeasurementDataRepository.Close();

            _processThread.Join(_PROCESS_THREAD_JOINT_TIMEOUT_MS);

            InitializationState = InitializationStates.NotInitialized;
        }

        #endregion

        #region IDataCollector

        public void GatherData(IToolSpecification specification, IEnumerable<string> measurementDataFileNames, IReferenceSample reference = null)
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return;
            }

            lock (_queueLockObj)
            {
                _processorQueue.Enqueue(new GetDataQueueElement(_parameters, specification, reference, measurementDataFileNames, ResultReadyEvent));
                _processQueueResetEvent.Set();
            }
        }

        public IEnumerable<IToolSpecification> GetAvailableToolSpecifications()
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return null;
            }

            IEnumerable<IToolSpecification> specifications = null;
            try
            {
                IEnumerable<object> specificationObjects = _parameters.SpecificationRepository.GetAllElements();
                specifications = specificationObjects.Cast<IToolSpecification>();
            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
            }
            return specifications;
        }


        public IEnumerable<IToolSpecification> GetSpecificationsByToolName(ToolNames toolName)
        {
            List<IToolSpecification> specList = null;
            try
            {
                IEnumerable<object> specificationObjects = _parameters.SpecificationRepository.GetAllElements();
                IEnumerable<IToolSpecification> specifications = specificationObjects.Cast<IToolSpecification>();
                specList = specifications.Where(p => p.ToolName == toolName).ToList();

            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
            }
            return specList;
        }


        public IEnumerable<IReferenceSample> GetReferenceSamples()
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return null;
            }

            IEnumerable<IReferenceSample> samples = null;
            try
            {
                IEnumerable<object> sampleObjects = _parameters.ReferenceRepository.GetAllElements();
                samples = sampleObjects.Cast<IReferenceSample>();

            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
            }
            return samples;
        }

        public string MeasurementFolderPath => PluginLoader.MeasurementDataFolder;

        #endregion

        #region private

        private void ProcessQueueElements(object obj)
        {
            _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} thread started.");

            CancellationToken token;
            try
            {
                token = (CancellationToken)obj;
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Received parameter is not {nameof(CancellationToken)}. Exception: {ex}");
                return;
            }

            WaitHandle[] handles = new WaitHandle[] { _processQueueResetEvent, token.WaitHandle };

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

                    QueueElement item;
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

                    item.Process();     // todo: exception védelem
                }


            }
        }

        #endregion

    }





    internal abstract class QueueElement
    {
        protected DataCollectorParameters _parameters;

        protected QueueElement(DataCollectorParameters parameters)
        {
            _parameters = parameters;
        }

        internal abstract void Process();
    }


    internal class GetDataQueueElement : QueueElement
    {
        private readonly IToolSpecification _specification;
        private readonly IReferenceSample _reference;
        private readonly IEnumerable<string> _measurementDataFileNames;
        private readonly EventHandler<ResultEventArgs> _resultReadyEvent;


        public GetDataQueueElement(DataCollectorParameters parameters, IToolSpecification specification, IReferenceSample reference, IEnumerable<string> measurementDataFileNames, EventHandler<ResultEventArgs> resultReadyEvent)
            : base(parameters)
        {
            _specification = specification;
            _reference = reference;
            _measurementDataFileNames = measurementDataFileNames;
            _resultReadyEvent = resultReadyEvent;
        }


        internal override void Process()
        {
            List<IToolMeasurementData> measurementDatas = new List<IToolMeasurementData>();
            foreach (string name in _measurementDataFileNames)
            {
                IToolMeasurementData measData = (IToolMeasurementData)_parameters.MeasurementDataRepository.Get(name);

                measurementDatas.Add(measData);
            }
            if (measurementDatas.Count < 1)
            {
                _parameters.Logger.LogError($"Number of measurement data files that meet the condition ({measurementDatas.Count}) is not acceptable.");
                return;
            }

            var localResultreadyevent = _resultReadyEvent;
            localResultreadyevent?.Invoke(this, new ResultEventArgs(new DataCollectorResult(_parameters.DateTimeProvider.GetDateTime(), true, _specification, measurementDatas, _reference)));
        }
    }

}
