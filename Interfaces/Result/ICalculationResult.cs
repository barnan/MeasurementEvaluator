using System;

namespace Interfaces.Result
{

    public interface ICalculationResult
    {
        DateTime CreationTime { get; }
    }

    public interface ISimpleCalculationResult : ICalculationResult
    {
        double Result { get; }
    }



}
