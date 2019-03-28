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

namespace DataAcquisitions.DataCollector
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
                    _processQueueResetEvent.Reset();
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
                                                 try
                                                 {
                                                     List<string> specificationcresult = _parameters.SpecificationRepository.GetAllNames().ToList();
                                                     List<string> referenceresult = _parameters.ReferenceRepository.GetAllNames().ToList();
                                                     List<string> measurementdata = _parameters.MeasurementDataRepository.GetAllNames().ToList();
                                                     if (specificationcresult.Count == 0)
                                                     {
                                                         _parameters.Logger.MethodError("Length of obtained SPECIFICATION list is zero or the list is null.");
                                                     }

                                                     if (measurementdata.Count == 0)
                                                     {
                                                         _parameters.Logger.MethodError("Length of obtained MEASUREMENT DATA list is zero or the list is null.");
                                                     }

                                                     if (referenceresult.Count == 0)
                                                     {
                                                         _parameters.Logger.MethodError("Length of obtained REFERENCE list is zero or the list is null.");
                                                     }


                                                     var filenamesreadyevent = FileNamesReadyEvent;
                                                     filenamesreadyevent?.Invoke(this, new DataCollectorResultEventArgs(specificationcresult, measurementdata, referenceresult));
                                                 }
                                                 catch (Exception ex)
                                                 {
                                                     _parameters.Logger.Error($"Exception occured during data request: {ex.Message}");
                                                 }
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
            var initializedEvent = Initialized;
            initializedEvent?.Invoke(this, new EventArgs());
        }


        private void OnClosed()
        {
            var closedEvent = Closed;
            closedEvent?.Invoke(this, new EventArgs());
        }

        #endregion

    }



    internal class QueueElement
    {
    }

    internal class GetDataQueueElement : QueueElement
    {
        private IDateTimeProvider _dateTimeProvider;

        internal List<string> MeasurementSerieDataNames { get; set; }
        internal string SpecificationName { get; set; }
        internal string ReferenceValueName { get; set; }



        //internal Action<string, List<string>, string> Process;
        internal void Process(string specName, List<string> MeasData, string reference)
        {
            DateTime startTime = _parameters.DateTimeProvider.GetDateTime();

            IToolSpecification specification = _parameters.SpecificationRepository.Get(specName);
            if (specification == null)
            {
                _parameters.Logger.LogInfo("There are no specification files with the given name arrived.");
                return;
            }

            List<IToolMeasurementData> measurementDatas = new List<IToolMeasurementData>();
            foreach (string name in measurementfilenames)
            {
                measurementDatas.Add(_parameters.MeasurementDataRepository.Get(name));
            }
            if (measurementDatas.Count < 1)
            {
                _parameters.Logger.LogError($"Number of measurement data files that meet the condition ({measurementDatas.Count}) is not acceptable.");
                return;
            }

            IReferenceSample reference = _parameters.ReferenceRepository.Get(refname);
            if (reference == null)
            {
                _parameters.Logger.LogInfo("There are no reference files with the given name or no reference name arrived.");
            }

            var localResultreadyevent = ResultReadyEvent;
            localResultreadyevent?.Invoke(this, new ResultEventArgs(new DataCollectorResult(startTime,
                                                                                        _parameters.DateTimeProvider.GetDateTime(),
                                                                                        true,
                                                                                        specification,
                                                                                        measurementDatas,
                                                                                        reference)));
        }
    }

    internal class GetNameQueueElement : QueueElement
    {
        //internal Action Process;
    }

}
