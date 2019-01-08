namespace Interfaces.ToolSpecifications
{
    public interface ISimpleCondition : ICondition<double>
    {
        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meets this relation -> this is the relation
        /// </summary>
        Relations ValidIf { get; }

        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meet this relation  -> this is the value
        /// </summary>
        double ValidIf_Value { get; }

    }
}
