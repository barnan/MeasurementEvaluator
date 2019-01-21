using Interfaces.MeasuredData;
using Interfaces.ToolSpecifications;
using System.Collections;

namespace Interfaces.Result
{

    public interface ICalculationResult : IResult
    {
        IToolMeasurementData MeasurementData { get; }

        IToolSpecification Specifiaction { get; }
    }



    public interface ICalculationResult<T> : ICalculationResult where T : struct
    {
        // TODO: possible development -> use NameData
        ArrayList Results { get; }
    }
}
