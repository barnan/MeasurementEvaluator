using Frame.MessageHandler;
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

            _parameters.Logger.LogMethodInfo("Instantiated.");
        }

        #endregion

        #region IResultProvider

        private event EventHandler<ResultEventArgs> ResultReadyEvent;

        public void SubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            ResultReadyEvent += method;

            _parameters.Logger.LogMethodInfo($"{method.GetMethodInfo().DeclaringType} class subscribed to {nameof(ResultReadyEvent)}");
        }

        public void UnSubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method)
        {
            ResultReadyEvent -= method;

            _parameters.Logger.LogMethodInfo($"{method.GetMethodInfo().DeclaringType} class un-subscribed to {nameof(ResultReadyEvent)}");
        }


        #endregion

        #region IInitializable


        protected override void InternalInit()
        {
            if (!_parameters.SpecificationRepository.Initiailze())
            {
                HandleInitializationFailed($"{_parameters.SpecificationRepository} could not been initialized.");

                return;
            }

            if (!_parameters.ReferenceRepository.Initiailze())
            {
                HandleInitializationFailed($"{_parameters.ReferenceRepository} could not been initialized.");
                return;
            }

            if (!_parameters.MeasurementDataRepository.Initiailze())
            {
                HandleInitializationFailed($"{_parameters.MeasurementDataRepository} could not been initialized.");
                return;
            }

            InitializationState = InitializationStates.Initialized;

            _parameters.MessageControl.AddMessage($"{_parameters.Name} initialized.");
        }

        private void HandleInitializationFailed(string message)
        {
            _parameters.MessageControl.AddMessage(_parameters.Logger.LogError(message), MessageSeverityLevels.Error);
            InitializationState = InitializationStates.InitializationFailed;
        }


        protected override void InternalClose()
        {
            _parameters.SpecificationRepository.Close();
            _parameters.ReferenceRepository.Close();
            _parameters.MeasurementDataRepository.Close();

            _processThread.Join(_PROCESS_THREAD_JOINT_TIMEOUT_MS);

            InitializationState = InitializationStates.NotInitialized;
        }

        #endregion

        #region IDataCollector

        public bool GatherData(IToolSpecification specification, IEnumerable<string> measurementDataFileNames, IReferenceSample reference = null)
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return false;
            }

            List<IToolMeasurementData> measurementDatas = new List<IToolMeasurementData>();
            foreach (string name in measurementDataFileNames)
            {
                measurementDatas.Add((IToolMeasurementData)_parameters.MeasurementDataRepository.Get(name));
            }

            var localResultreadyevent = ResultReadyEvent;
            localResultreadyevent?.Invoke(this, new ResultEventArgs(new DataCollectorResult(_parameters.DateTimeProvider.GetDateTime(), true, specification, measurementDatas, reference)));

            return true;
        }


        public IEnumerable<IToolSpecification> GetAvailableToolSpecifications()
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return null;
            }

            try
            {
                IEnumerable<object> specificationObjects = _parameters.SpecificationRepository.GetAllElements();
                return specificationObjects.Cast<IToolSpecification>();
            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
                return null;
            }
        }


        public IEnumerable<IToolSpecification> GetSpecificationsByToolName(ToolNames toolName)
        {
            try
            {
                IEnumerable<object> specificationObjects = _parameters.SpecificationRepository.GetAllElements();
                IEnumerable<IToolSpecification> specifications = specificationObjects.Cast<IToolSpecification>();
                return specifications.Where(p => p.ToolName == toolName).ToList();
            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
                return null;
            }
        }


        public IEnumerable<IReferenceSample> GetAvailableReferenceSamples()
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return null;
            }

            try
            {
                IEnumerable<object> sampleObjects = _parameters.ReferenceRepository.GetAllElements();
                return sampleObjects.Cast<IReferenceSample>();

            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
                return null;
            }
        }

        public string MeasurementFolderPath => PluginLoader.MeasurementDataFolder;

        #endregion

    }

}
