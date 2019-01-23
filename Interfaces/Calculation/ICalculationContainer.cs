using System.Collections.Generic;

namespace Interfaces.Calculation
{
    public interface ICalculationContainer
    {
        ICalculation GetCalculation(CalculationTypes calculationType);

        IReadOnlyList<CalculationTypes> AvailableCalculatons { get; }
    }
}
