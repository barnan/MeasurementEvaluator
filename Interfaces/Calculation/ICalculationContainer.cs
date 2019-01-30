using System.Collections.Generic;

namespace Interfaces.Calculation
{
    public interface ICalculationContainer
    {
        ICalculation GetCalculation(CalculationTypes requiredCalculationType);

        IReadOnlyList<CalculationTypes> AvailableCalculatons { get; }
    }
}
