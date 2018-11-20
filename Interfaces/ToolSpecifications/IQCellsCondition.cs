namespace Interfaces.ToolSpecifications
{
    public interface ICpkCondition<T> : ICondition<T>
    {
        /// <summary>
        /// Half of the tolerance interval
        /// </summary>
        double HalfTolerance { get; }

        /// <summary>
        /// Defines whether relaite or absolute condition
        /// </summary>
        RELATIVEORABSOLUTE RelOrAbs { get; }
    }
}
