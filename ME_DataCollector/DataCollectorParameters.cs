﻿using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using Interfaces.Misc;
using NLog;

namespace MeasurementEvaluator.ME_DataCollector
{
    internal class DataCollectorParameters
    {

        internal ILogger Logger { get; private set; }

        [Configuration("Name of date and time provider", "Date and time Provider", true)]
        private string _dateTimeProviderName = null;
        internal IDateTimeProvider DateTimeProvider { get; private set; }

        [Configuration("Name of the measurement Data repository", "Measurement Data Repository", LoadComponent = true)]
        private string _measurementDataRepositoryName = null;
        internal IRepository MeasurementDataRepository { get; private set; }

        [Configuration("Name of the reference repository", "Reference Repository", LoadComponent = true)]
        private string _referenceRepositoryName = null;
        internal IRepository ReferenceRepository { get; private set; }

        [Configuration("Name of the specification repository", "Specification Repository", LoadComponent = true)]
        private string _specificationRepositoryName = null;
        internal IRepository SpecificationRepository { get; private set; }



        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetLogger(sectionName);

            PluginLoader.ConfigManager.Load(this, sectionName);

            SpecificationRepository = PluginLoader.CreateInstance<IRepository>(_specificationRepositoryName);
            ReferenceRepository = PluginLoader.CreateInstance<IRepository>(_referenceRepositoryName);
            MeasurementDataRepository = PluginLoader.CreateInstance<IRepository>(_measurementDataRepositoryName);
            DateTimeProvider = PluginLoader.CreateInstance<IDateTimeProvider>(_dateTimeProviderName);

            return CheckComponent();
        }

        private bool CheckComponent()
        {
            if (SpecificationRepository == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} loading. {nameof(SpecificationRepository)} is null.");
                return false;
            }

            if (ReferenceRepository == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} loading. {nameof(ReferenceRepository)} is null.");
                return false;
            }


            if (MeasurementDataRepository == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} loading. {nameof(MeasurementDataRepository)} is null.");
                return false;
            }

            if (DateTimeProvider == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} loading. {nameof(DateTimeProvider)} is null.");
                return false;
            }

            return true;
        }
    }
}
