using Interfaces.BaseClasses;
using Interfaces.Calculation;
using Miscellaneous;
using System.Collections.Generic;
using System.Linq;

namespace Calculations.CalculationContainer
{
    internal class CalculationContainer : ICalculationContainer
    {
        private readonly CalculationContainerParameters _parameters;

        public IReadOnlyList<CalculationTypesValues> AvailableCalculatons
        {
            get
            {
                _parameters.Logger.LogInfo($"{nameof(AvailableCalculatons)} request arrived.");

                return _parameters.AvailableCalculations.Select(p => p.CalculationType).ToList().AsReadOnly();
            }
        }


        public ICalculation GetCalculation(CalculationTypes requiredCalculationType)
        {
            _parameters.Logger.LogInfo($"Request for {requiredCalculationType} typed calculation.");

            if ((_parameters.AvailableCalculations?.Count ?? 0) == 0)
            {
                _parameters.Logger.LogError($"{nameof(_parameters.AvailableCalculations)} is null or empty.");
            }

            List<ICalculation> calcList = _parameters.AvailableCalculations.Where(p => p.CalculationType == requiredCalculationType.CalculationTypeValue).ToList();

            if (calcList.Count == 0)
            {
                _parameters.Logger.LogError($"The available calculations list does not contain element: {requiredCalculationType}");
                return null;
            }

            if (calcList.Count > 1)
            {
                _parameters.Logger.LogError($"The available calculations list contains more element of: {requiredCalculationType}");
                return null;
            }

            return calcList[0];
        }


        public CalculationContainer(CalculationContainerParameters parameters)
        {
            _parameters = parameters;
            _parameters.Logger.Info($"{nameof(CalculationContainer)} created.");
        }

    }
}
