﻿using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.BaseClasses;
using Interfaces.Misc;
using NLog;

namespace Calculations.Calculation
{
    internal class CalculationParameters
    {
        public ILogger Logger { get; private set; }


        [Configuration("DateTimeProvider", "DateTimeProvider", LoadComponent = true)]
        private string _dateTimeProviderName = null;
        public IDateTimeProvider DateTimeProvider;


        [Configuration("CalculationType", "CalculationType", LoadComponent = true)]
        private CalculationType _calculationType = CalculationType.Unknown;
        public CalculationType CalculationTypeValue => _calculationType;


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetLogger(sectionName);

            PluginLoader.ConfigManager.Load(this, sectionName);
            DateTimeProvider = PluginLoader.CreateInstance<IDateTimeProvider>(_dateTimeProviderName);

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (DateTimeProvider == null)
            {
                Logger.Error($"Error in the {nameof(CalculationParameters)} loading. {nameof(DateTimeProvider)} is null.");
                return false;
            }

            return true;
        }
    }
}
