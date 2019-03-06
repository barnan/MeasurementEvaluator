namespace Interfaces.ToolSpecifications
{
    public interface ISimpleCondition : ICondition<double>
    {
        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meets this relation
        /// </summary>
        Relations ValidIf { get; }

        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meets this Validif condition with the ValidIfValue value
        /// </summary>
        double ValidIf_Value { get; }
    }


    public interface ISimpleConditionHandler : ISimpleCondition
    {
        Relations ValidIf { get; set; }

        double ValidIf_Value { get; set; }
    }

}
