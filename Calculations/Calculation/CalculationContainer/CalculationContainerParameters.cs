﻿


using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Calculation;
using NLog;
using System.Collections.Generic;

namespace Calculations.Calculation.CalculationContainer
{
    internal class CalculationContainerParameters
    {
        [Configuration("AvailableCalculation", Name = "AvailableCalculation", LoadComponent = false)]
        private List<string> _availableCalculationsString = null;


        private List<ICalculation> _availableCalculations;
        public IReadOnlyList<ICalculation> AvailableCalculations
        {
            get { return _availableCalculations.AsReadOnly(); }
        }


        public ILogger Logger { get; internal set; }


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            foreach (string calculationName in _availableCalculationsString)
            {
                ICalculation calculation = PluginLoader.CreateInstance<ICalculation>(calculationName);

                if (calculation == null)
                {
                    continue;
                }
                _availableCalculations.Add(calculation);
            }

            return CheckComponents();
        }


        private bool CheckComponents()
        {
            if (AvailableCalculations == null)
            {
                Logger.Error($"Error in the {nameof(CalculationContainerParameters)} instantiation. {nameof(AvailableCalculations)} is null.");
                return false;
            }

            return true;
        }
    }
}
