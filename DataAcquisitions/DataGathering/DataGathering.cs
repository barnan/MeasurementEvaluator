using Interfaces;
using Interfaces.DataAcquisition;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAcquisitions.DataGathering
{
    internal class DataGathering : IDataGathering
    {

        private readonly DataGatheringParameters _parameters;
        private readonly object _lockObj = new object();


        public DataGathering(DataGatheringParameters dataGatheringParameters)
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

        #region IDataGathering

        public void Gather(string specifactionName, List<string> measurementDataFileNames, string referenceName = null)
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

                    var specifications = _parameters.SpecificationRepository.GetAll();
                    var references = _parameters.ReferenceRepository.GetAll();
                    var measurementDatas = _parameters.MeasurementDataRepository.GetAll();




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

    }
}
