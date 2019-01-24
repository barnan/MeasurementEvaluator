namespace Interfaces.Calculation
{
    public interface ICalculationSettings
    {
    }


    public interface ICpkCalculationSettings : ICalculationSettings
    {
        double HalfTolerance { get; }

        double ReferenceValue { get; }
    }


    public interface ICpCalculationSettings : ICalculationSettings
    {
        double HalfTolerance { get; }
    }

}
