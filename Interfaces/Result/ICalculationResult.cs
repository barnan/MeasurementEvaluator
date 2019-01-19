using Interfaces.MeasuredData;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace Interfaces.Result
{

    public interface ICalculationResult : IResult
    {
        IToolMeasurementData MeasurementData { get; }

        IToolSpecification Specifiaction { get; }
    }



    public interface ICalculationResult<T> : ICalculationResult where T : struct
    {
        IReadOnlyList<T> Results { get; }
    }
}
