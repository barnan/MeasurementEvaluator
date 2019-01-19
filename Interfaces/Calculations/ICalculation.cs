using Interfaces.Misc;
using Interfaces.Result;
using System.Collections.Generic;

namespace Interfaces.Calculations
{

    public interface ICalculation : IInitializable, IResultProvider
    {
        IReadOnlyList<CalculationTypes> AvailableCalculationTypes { get; }

    }




}
