using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataAcquisitions.DataCollector
{
    internal class DataCollector : IDataCollector
    {

        private readonly DataCollectorParameters _parameters;
        private readonly object _lockObj = new object();
        private Queue<QueueElement> _processorQueue;
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private CancellationTokenSource _tokenSource;
        private const int WAITHANDLE_CYCLETIME_MS = 100;


        public DataCollector(DataCollectorParameters dataGatheringParameters)
        {
            _parameters = dataGatheringParameters;
            _parameters.Logger.MethodInfo("Instantiated.");
        }


        #region IResultProvider

        public event EventHandler<ResultEventArgs> ResultReadyEvent;

        #endregion


        #region IInitializable

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
                try
                {
                    if (!IsInitialized)
                    {
                        return;
                    }

                    _tokenSource.Cancel();
                    _resetEvent.Reset();
                    _processorQueue.Clear();

                    _parameters.SpecificationRepository.Close();
                    _parameters.ReferenceRepository.Close();
                    _parameters.MeasurementDataRepository.Close();

                    IsInitialized = false;
                    OnClosed();
                    _parameters.Logger.MethodInfo("Closed.");
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                }
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
                try
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

                    IsInitialized = true;
                    OnInitialized();
                    _parameters.Logger.MethodInfo("Initialized.");

                    return IsInitialized;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                    return IsInitialized = false;
                }
            }
        }

        #endregion

        #region IDataCollector

        public event EventHandler<DataCollectorResultEventArgs> FileNamesReadyEvent;


        public void GatherData(string specificationName, List<string> measurementDataFileNames, string referenceName = null)
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsInitialized)
                    {
                        _parameters.Logger.LogError("Not initialized yet.");
                        return;
                    }

                    _processorQueue.Enqueue(new GetDataQueueElement
                    {
                        SpecificationName = specificationName,
                        ReferenceValueName = referenceName,
                        MeasurementSerieDataNames = measurementDataFileNames,

                        // create anonim method to process itself as queue element
                        Process = delegate (string specName, List<string> measurementfilenames, string refname)
                        {
                            DateTime startTime = _parameters.DateTimeProvider.GetDateTime();

                            List<IToolSpecification> specification = _parameters.SpecificationRepository.GetAll().Where(p => string.Equals(p.FullNameOnHDD, specName)).ToList();
                            List<IToolMeasurementData> measurementDatas = _parameters.MeasurementDataRepository.GetAll().Where(p => measurementfilenames.Contains(p.FullNameOnHDD)).ToList();

                            IReferenceSample referenceToSend = null;
                            List<IReferenceSample> references = _parameters.ReferenceRepository.GetAll().Where(p => string.Equals(p.FullNameOnHDD, refname)).ToList();

                            if (specification.Count != 1)
                            {
                                _parameters.Logger.LogError($"Number of specification files that meet the condition ({specification.Count}) is not acceptable.");
                                return;
                            }

                            if (measurementDatas.Count < 1)
                            {
                                _parameters.Logger.LogError($"Number of measurement data files that meet the condition ({measurementDatas.Count}) is not acceptable.");
                                return;
                            }

                            if ((references?.Count ?? 0) == 0)
                            {
                                _parameters.Logger.LogInfo("There are no reference files with the given name or no reference name arrived.");
                            }
                            else
                            {
                                referenceToSend = references[0];
                            }

                            var resultreadyevent = ResultReadyEvent;
                            resultreadyevent?.Invoke(this, new ResultEventArgs(new DataCollectorResult(startTime,
                                _parameters.DateTimeProvider.GetDateTime(),
                                true,
                                specification[0],
                                measurementDatas,
                                referenceToSend)));

                        }
                    });

                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                }
            }
        }


        public void GatherNames()
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsInitialized)
                    {
                        _parameters.Logger.LogError("Not initialized yet.");
                        return;
                    }

                    _processorQueue.Enqueue(new GetNameQueueElement
                    {
                        Process = delegate ()
                                             {
                                                 List<string> specificationcresult = _parameters.SpecificationRepository.GetAll().Select(p => p.SpecificationName.ToString()).ToList();
                                                 List<string> referenceresult = _parameters.ReferenceRepository.GetAll().Select(p => p.SampleID.ToString()).ToList();
                                                 List<string> measurementdata = _parameters.MeasurementDataRepository.GetAll().Select(p => p.FullNameOnHDD.ToString()).ToList();

                                                 if ((specificationcresult?.Count ?? 0) == 0)
                                                 {
                                                     _parameters.Logger.MethodError("Length of obtained SPECIFICATION list is zero or the list is null.");
                                                 }

                                                 if ((measurementdata?.Count ?? 0) == 0)
                                                 {
                                                     _parameters.Logger.MethodError("Length of obtained MEASUREMENT DATA list is zero or the list is null.");
                                                 }

                                                 if ((referenceresult?.Count ?? 0) == 0)
                                                 {
                                                     _parameters.Logger.MethodError("Length of obtained REFERENCE list is zero or the list is null.");
                                                 }


                                                 var filenamesreadyevent = FileNamesReadyEvent;
                                                 filenamesreadyevent?.Invoke(this, new DataCollectorResultEventArgs(specificationcresult, measurementdata, referenceresult));
                                             }
                    });
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                    return;
                }
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
                if (_resetEvent.WaitOne(WAITHANDLE_CYCLETIME_MS))
                {
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
                            break;
                        }

                        QueueElement item = null;
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

                        if (item is GetNameQueueElement getNameQueueElement)
                        {
                            getNameQueueElement.Process();
                        }

                        if (item is GetDataQueueElement getDataQueueElement)
                        {
                            getDataQueueElement.Process(getDataQueueElement.SpecificationName, getDataQueueElement.MeasurementSerieDataNames, getDataQueueElement.ReferenceValueName);
                        }
                    }
                }

                if (token.IsCancellationRequested)
                {
                    _parameters.Logger.MethodInfo($"{Thread.CurrentThread.Name} (ID:{Thread.CurrentThread.ManagedThreadId}) thread cancelled.");
                    break;
                }
            }

        }



        private void OnInitialized()
        {
            var initialized = Initialized;

            initialized?.Invoke(this, new EventArgs());
        }


        private void OnClosed()
        {
            var closed = Closed;

            closed?.Invoke(this, new EventArgs());
        }

        #endregion

    }



    internal class QueueElement
    {
    }

    internal class GetDataQueueElement : QueueElement
    {
        internal List<string> MeasurementSerieDataNames { get; set; }
        internal string SpecificationName { get; set; }
        internal string ReferenceValueName { get; set; }

        internal Action<string, List<string>, string> Process;
    }

    internal class GetNameQueueElement : QueueElement
    {
        internal Action Process;
    }

}
