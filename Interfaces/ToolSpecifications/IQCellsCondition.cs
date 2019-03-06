namespace Interfaces.ToolSpecifications
{
    public interface ICpkCondition : ICondition<double>
    {
        /// <summary>
        /// Half of the tolerance interval
        /// </summary>
        double HalfTolerance { get; }

        /// <summary>
        /// Defines whether relative or absolute condition
        /// </summary>
        RELATIVEORABSOLUTE RelOrAbs { get; }
    }



    public interface ICpkConditionHandler : ICpkCondition
    {
        new double HalfTolerance { get; set; }

        new RELATIVEORABSOLUTE RelOrAbs { get; set; }
    }

}
