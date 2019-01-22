using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace Interfaces.Calculation
{

    public interface ICalculation : IInitializable, IResultProvider
    {
        IReadOnlyList<CalculationTypes> AvailableCalculationTypes { get; }
    }


    public interface IConditionCalculation : IResultProvider
    {
        bool Calculate(IToolMeasurementData measurementdata, ICondition condition, IReferenceSample referencesample);

        CalculationTypes CalculationType { get; }
    }

}
