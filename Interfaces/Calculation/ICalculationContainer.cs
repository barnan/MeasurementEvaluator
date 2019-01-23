using System.Collections.Generic;

namespace Interfaces.Calculation
{
    public interface ICalculationContainer
    {
        ICalculation GetCalculation(CalculationTypes calculationType);

        IReadOnlyList<ICalculation> AvailableCalculatons { get; }
    }
}
