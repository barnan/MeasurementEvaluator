


using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Calculation;
using NLog;
using System.Collections.Generic;

namespace Calculations.CalculationContainer
{
    internal class CalculationContainerParameters
    {
        [Configuration("AvailableCalculation", "AvailableCalculation", LoadComponent = false)]
        private List<string> _availableCalculationsList = new List<string>();


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

            _availableCalculations = new List<ICalculation>();
            foreach (string calculationName in _availableCalculationsList)
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

            if (AvailableCalculations.Count == 0)
            {
                Logger.Error($"Error in the {nameof(CalculationContainerParameters)} instantiation. {nameof(AvailableCalculations)} has 0 elements.");
                return false;
            }

            return true;
        }
    }
}
