using Interfaces.BaseClasses;
using System.Collections.Generic;

namespace Interfaces.Calculation
{
    public interface ICalculationContainer
    {
        /// <summary>
        /// Get a given calculation from the container
        /// </summary>
        /// <param name="requiredCalculationType">type of the required calculation</param>
        /// <returns>the calculation object itself</returns>
        ICalculation GetCalculation(CalculationTypes requiredCalculationType);


        /// <summary>
        /// Gives back list of all avilable calculations in the container
        /// </summary>
        IReadOnlyList<CalculationTypes> AvailableCalculatons { get; }
    }
}
