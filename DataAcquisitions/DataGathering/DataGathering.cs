using Interfaces;
using Interfaces.DataAcquisition;
using Miscellaneous;
using System;

namespace DataAcquisitions.DataGathering
{
    internal class DataGathering : IDataCollector
    {
        private DataGatheringParameters _parameters;
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
                if (!IsInitialized)
                {
                    return;
                }

                _parameters.SpecificationRepository.Close();
                _parameters.ReferenceRepository.Close();
                _parameters.MeasurementDataRepository.Close();

                IsInitialized = false;

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

                IsInitialized = true;

                _parameters.Logger.MethodInfo("Initialized.");

                return IsInitialized;
            }
        }

        #endregion

        #region IDataCollector

        public void Gather()
        {



        }

        #endregion
    }
}
