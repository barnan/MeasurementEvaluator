using System;

namespace Interfaces.Result
{

    public interface ICalculationResult : ISaveableResult
    {
        DateTime CreationTime { get; }
    }



    public interface ISimpleCalculationResult : ICalculationResult
    {
        double Result { get; }
    }


    public interface IQCellsCalculationResult : ICalculationResult
    {
        double Result { get; }

        double USL { get; }

        double LSL { get; }
    }



}
