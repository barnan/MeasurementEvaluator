namespace Measurement_Evaluator.Calculation
{
    public abstract class CalculationBase<T, U>
    {
        public T Config { get; set; }
        public abstract void ExecuteOn(U Data);

    }
}
