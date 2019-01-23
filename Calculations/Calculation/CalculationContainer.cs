using Interfaces;
using Interfaces.Calculation;
using Miscellaneous;
using System.Collections.Generic;
using System.Linq;

namespace Calculations.Calculation
{
    internal class CalculationContainer : ICalculationContainer
    {
        CalculationContainerParameters _parameters;

        public IReadOnlyList<CalculationTypes> AvailableCalculatons
        {
            get
            {
                _parameters.Logger.LogInfo("AvilableCalculations request arrived.");

                return _parameters.AvailableCalculations.Select(p => p.CalculationType).ToList().AsReadOnly();
            }
        }


        public ICalculation GetCalculation(CalculationTypes calculationType)
        {
            _parameters.Logger.LogInfo($"Request for {calculationType} typed calculation.");

            if ((_parameters.AvailableCalculations?.Count ?? 0) == 0)
            {
                _parameters.Logger.LogError($"{nameof(_parameters.AvailableCalculations)} is null or empty.");
            }

            List<ICalculation> calcList = _parameters.AvailableCalculations.Where(p => p.CalculationType == calculationType).ToList();

            if (calcList.Count == 0)
            {
                _parameters.Logger.LogError($"The available calculations list does not contain element: {calculationType}");
                return null;
            }

            if (calcList.Count > 1)
            {
                _parameters.Logger.LogError($"The available calculations list contains more element of: {calculationType}");
                return null;
            }

            return calcList[0];
        }


        public CalculationContainer(CalculationContainerParameters parameters)
        {
            _parameters = parameters;
        }

    }
}
