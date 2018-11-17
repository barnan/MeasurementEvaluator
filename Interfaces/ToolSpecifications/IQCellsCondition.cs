namespace Measurement_Evaluator.Interfaces.ToolSpecifications
{
    public interface ICpkCondition : ICondition
    {
        /// <summary>
        /// Half of the tolerance interval
        /// </summary>
        double HalfTolerance { get; set; }

        /// <summary>
        /// Defines whether relaite or absolute condition
        /// </summary>
        RELATIVEORABSOLUTE RelOrAbs { get; set; }
    }
}
