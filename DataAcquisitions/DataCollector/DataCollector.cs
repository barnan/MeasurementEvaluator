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
        private Queue<QueueElement> _processQueue;
        private AutoResetEvent _calculationResetEvent = new AutoResetEvent(false);
        private CancellationTokenSource _tokenSource;


        public DataCollector(DataCollectorParameters dataGatheringParameters)
        {
            _parameters = dataGatheringParameters;
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
                    _processQueue.Clear();

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

                    _processQueue = new Queue<QueueElement>();
                    _tokenSource = new CancellationTokenSource();
                    Thread th = new Thread(CalculatorThread)
                    {
                        IsBackground = true,
                        Name = "CalculatorThread"
                    };
                    th.Start(_tokenSource);

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

        public void Gather(string specificationName, List<string> measurementDataFileNames, string referenceName = null)
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

                    DateTime startTime = _parameters.DateTimeProvider.GetDateTime();

                    List<IToolSpecification> specification = _parameters.SpecificationRepository.GetAll().Where(p => string.Equals(p.FullNameOnHDD, specificationName)).ToList();
                    List<IToolMeasurementData> measurementDatas = _parameters.MeasurementDataRepository.GetAll().Where(p => measurementDataFileNames.Contains(p.FullNameOnHDD)).ToList();

                    IReferenceSample referenceToSend = null;
                    List<IReferenceSample> references = _parameters.ReferenceRepository.GetAll().Where(p => string.Equals(p.FullNameOnHDD, referenceName)).ToList();

                    if (specification.Count != 1)
                    {
                        _parameters.Logger.LogError("Number of specification files that meet the condition is not acceptable.");
                        return;
                    }

                    if (measurementDatas.Count < 1)
                    {
                        _parameters.Logger.LogError("Number of measurement data files that meet the condition is not acceptable.");
                        return;
                    }

                    if ((references?.Count ?? 0) == 0)
                    {
                        _parameters.Logger.LogInfo("There are no reference files with the given nam, or no reference name arrived.");
                    }
                    else
                    {
                        referenceToSend = references[0];
                    }


                    // TODO: send invalid result
                    var resultreadyevent = ResultReadyEvent;
                    resultreadyevent?.Invoke(this, new ResultEventArgs(new DataCollectorResult(startTime,
                        _parameters.DateTimeProvider.GetDateTime(),
                        true,
                        specification[0],
                        measurementDatas,
                        referenceToSend)));

                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                }
            }
        }


        public IReadOnlyList<string> GetAllSpecificationNames()
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsInitialized)
                    {
                        _parameters.Logger.LogError("Not initialized yet.");
                        return null;
                    }

                    var result = _parameters.SpecificationRepository.GetAll().Select(p => p.SpecificationName.ToString()).ToList();

                    return result.AsReadOnly();
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                    return null;
                }
            }
        }


        public IReadOnlyList<string> GetAllRferenceSampleNames()
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsInitialized)
                    {
                        _parameters.Logger.LogError("Not initialized yet.");
                        return null;
                    }

                    var result = _parameters.ReferenceRepository.GetAll().Select(p => p.SampleID.ToString()).ToList();

                    return result.AsReadOnly();
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                    return null;
                }
            }
        }


        public IReadOnlyList<string> GetAllMeasurementFileNames()
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsInitialized)
                    {
                        _parameters.Logger.LogError("Not initialized yet.");
                        return null;
                    }

                    var result = _parameters.MeasurementDataRepository.GetAll().Select(p => p.FullNameOnHDD.ToString()).ToList();

                    return result.AsReadOnly();
                }
                catch (Exception ex)
                {
                    _parameters.Logger.LogError($"Exception occured: {ex}");
                    return null;
                }
            }
        }


        #endregion


        #region private

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


        private void CalculatorThread(object obj)
        {
            CancellationToken token = (CancellationToken)obj;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    _parameters.Logger.LogError($"Thread: {Thread.CurrentThread.Name} ({Thread.CurrentThread.ManagedThreadId}) cancelled.");
                    break;
                }




            }
        }

        #endregion

    }



    internal class QueueElement
    {
        IMeasurementSerie MeasurementSerieData { get; }
        ICondition Condition { get; }
        IReferenceValue ReferenceValue { get; }
    }

}
