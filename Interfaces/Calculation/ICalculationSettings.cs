using Interfaces.BaseClasses;

namespace Interfaces.Calculation
{

    /// <summary>
    /// 
    /// </summary>
    public interface ICalculationSettings
    {
        CalculationType ValidCalculation { get; }
    }


    /// <summary>
    /// settings for the calculation, the values are from the specification
    /// </summary>
    public interface ICpkCalculationSettings : ICalculationSettings
    {
        double HalfTolerance { get; }

        double ReferenceValue { get; }
    }


    /// <summary>
    /// settings for the calculation, the values are from the specification
    /// </summary>
    public interface ICpCalculationSettings : ICalculationSettings
    {
        double HalfTolerance { get; }
    }

}
