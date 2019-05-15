using Frame.PluginLoader;
using Interfaces;
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
    internal class DataCollector : IDataCollector
    {

        private readonly DataCollectorParameters _parameters;
        private readonly object _lockObj = new object();

        private Queue<QueueElement> _processorQueue;
        private AutoResetEvent _processQueueResetEvent = new AutoResetEvent(false);
        private CancellationTokenSource _tokenSource;
        private const int WAITHANDLE_CYCLETIME_MS = 100;


        public DataCollector(DataCollectorParameters dataGatheringParameters)
        {
            _parameters = dataGatheringParameters;
            _parameters.Logger.MethodInfo("Instantiated.");

            _parameters.SpecificationRepository.SetFolder(PluginLoader.SpecificationFolder);
            _parameters.ReferenceRepository.SetFolder(PluginLoader.ReferenceFolder);
            _parameters.MeasurementDataRepository.SetFolder(PluginLoader.MeasurementDataFolder);
        }


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

        #region IInitializable

        public bool IsInitialized { get; private set; }

        public event EventHandler<InitializationEventArgs> InitStateChanged;

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

                _tokenSource.Cancel();
                _processQueueResetEvent.Reset();
                _processorQueue.Clear();

                _parameters.SpecificationRepository.Close();
                _parameters.ReferenceRepository.Close();
                _parameters.MeasurementDataRepository.Close();

                bool oldInitState = IsInitialized;
                IsInitialized = false;
                OnInitStateChanged(IsInitialized, oldInitState);

                _parameters.Logger.MethodInfo("Closed.");
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

                if (!_parameters.SpecificationRepository.Initiailze())
                {
                    _parameters.Logger.LogError($"{_parameters.SpecificationRepository} could not been initialized.");

                    return false;
                }

                if (!_parameters.ReferenceRepository.Initiailze())
                {
                    _parameters.Logger.LogError($"{_parameters.ReferenceRepository} could not been initialized.");
                    return false;
                }

                if (!_parameters.MeasurementDataRepository.Initiailze())
                {
                    _parameters.Logger.LogError($"{_parameters.MeasurementDataRepository} could not been initialized.");
                    return false;
                }

                _processorQueue = new Queue<QueueElement>();
                _tokenSource = new CancellationTokenSource();

                // start queue procesor thread:
                Thread thread = new Thread(ProcessQueueElements)
                {
                    IsBackground = false,
                    Name = "DataCollectorQueueElementProcessor"
                };
                thread.Start(_tokenSource.Token);

                bool oldInitState = IsInitialized;
                IsInitialized = true;
                OnInitStateChanged(IsInitialized, oldInitState);

                _parameters.Logger.MethodInfo("Initialized.");

                return IsInitialized;
            }
        }

        private void OnInitStateChanged(bool newState, bool oldState)
        {
            var initialized = InitStateChanged;
            initialized?.Invoke(this, new InitializationEventArgs(newState, oldState));
        }

        #endregion

        #region IDataCollector

        public void GatherData(string specificationName, List<string> measurementDataFileNames, string referenceName = null)
        {
            lock (_lockObj)
            {
                if (!IsInitialized)
                {
                    _parameters.Logger.LogError("Not initialized yet.");
                    return;
                }

                _processorQueue.Enqueue(new GetDataQueueElement(_parameters, specificationName, referenceName, measurementDataFileNames, ResultReadyEvent));

            }
        }

        public List<ToolNames> GetAvailableToolNames()
        {
            lock (_lockObj)
            {
                IEnumerable<string> specificationNames = _parameters.SpecificationRepository.GetAllNames();
                List<ToolNames> toolList = new List<ToolNames>();

                foreach (string specificationName in specificationNames)
                {
                    try
                    {
                        var specification = (IToolSpecification)_parameters.SpecificationRepository.Get(specificationName);
                        if (!toolList.Contains(specification.ToolName))
                        {
                            toolList.Add(specification.ToolName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _parameters.Logger.Error($"Exception occured: {ex}");
                    }
                }

                return toolList;
            }
        }


        public List<IToolSpecification> GetSpecifications(ToolNames toolName)
        {
            lock (_lockObj)
            {
                IEnumerable<string> specificationNames = _parameters.SpecificationRepository.GetAllNames();
                List<IToolSpecification> specList = new List<IToolSpecification>();

                foreach (string specificationName in specificationNames)
                {
                    try
                    {
                        var specification = (IToolSpecification)_parameters.SpecificationRepository.Get(specificationName);
                        if (specList.Any(p => p.Name == specification.Name))
                        {
                            _parameters.Logger.Error($"The given specification name is already added: {specification.Name}");
                            continue;
                        }
                        specList.Add(specification);
                    }
                    catch (Exception ex)
                    {
                        _parameters.Logger.Error($"Exception occured: {ex}");
                    }
                }
                return specList;
            }
        }


        public List<IReferenceSample> GetReferenceSamples()
        {
            lock (_lockObj)
            {
                IEnumerable<string> sampleNames = _parameters.ReferenceRepository.GetAllNames();
                List<IReferenceSample> refList = new List<IReferenceSample>();

                foreach (string sampleName in sampleNames)
                {
                    try
                    {
                        var reference = (IReferenceSample)_parameters.ReferenceRepository.Get(sampleName);
                        if (refList.Any(p => p.Name == reference.Name))
                        {
                            _parameters.Logger.Error($"The given specification name is already added: {reference.Name}");
                            continue;
                        }
                        refList.Add(reference);
                    }
                    catch (Exception ex)
                    {
                        _parameters.Logger.Error($"Exception occured: {ex}");
                    }
                }
                return refList;
            }
        }

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
                _parameters.Logger.MethodError($"Arrived parameter is not {nameof(CancellationToken)}. Exception: {ex}");
                return;
            }

            while (true)
            {
                if (_processQueueResetEvent.WaitOne(WAITHANDLE_CYCLETIME_MS))
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
                            break;
                        }

                        QueueElement item;
                        lock (_lockObj)
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

                        item.Process();
                    }
                }

                if (token.IsCancellationRequested)
                {
                    _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
                    break;
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
        private string _specificationName;
        private string _referenceName;
        private List<string> _measurementDataFileNames;
        private EventHandler<ResultEventArgs> _resultReadyEvent;


        public GetDataQueueElement(DataCollectorParameters parameters, string specificationName, string referenceName, List<string> measurementDataFileNames, EventHandler<ResultEventArgs> resultReadyEvent)
            : base(parameters)
        {
            _specificationName = specificationName;
            _referenceName = referenceName;
            _measurementDataFileNames = measurementDataFileNames;
            _resultReadyEvent = resultReadyEvent;
        }


        internal override void Process()
        {
            DateTime startTime = _parameters.DateTimeProvider.GetDateTime();

            IToolSpecification specification = (IToolSpecification)_parameters.SpecificationRepository.Get(_specificationName);
            if (specification == null)
            {
                _parameters.Logger.LogInfo("There are no specification files with the given name arrived.");
                return;
            }

            List<IToolMeasurementData> measurementDatas = new List<IToolMeasurementData>();
            foreach (string name in _measurementDataFileNames)
            {
                measurementDatas.Add((IToolMeasurementData)_parameters.MeasurementDataRepository.Get(name));
            }
            if (measurementDatas.Count < 1)
            {
                _parameters.Logger.LogError($"Number of measurement data files that meet the condition ({measurementDatas.Count}) is not acceptable.");
                return;
            }

            IReferenceSample reference = (IReferenceSample)_parameters.ReferenceRepository.Get(_referenceName);
            if (reference == null)
            {
                _parameters.Logger.LogInfo("There are no reference files with the given name or no reference name arrived.");
            }

            var localResultreadyevent = _resultReadyEvent;
            localResultreadyevent?.Invoke(this, new ResultEventArgs(new DataCollectorResult(startTime,
                                                                                        _parameters.DateTimeProvider.GetDateTime(),
                                                                                        true,
                                                                                        specification,
                                                                                        measurementDatas,
                                                                                        reference)));
        }
    }


}
