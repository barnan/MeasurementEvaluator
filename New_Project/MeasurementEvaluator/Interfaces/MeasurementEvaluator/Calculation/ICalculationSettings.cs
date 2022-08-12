
namespace Interfaces.MeasurementEvaluator.Calculation
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICalculationSettings
    {
        CalculationTypes CalculationType { get; }
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
