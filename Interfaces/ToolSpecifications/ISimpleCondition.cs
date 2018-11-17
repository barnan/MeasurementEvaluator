namespace Measurement_Evaluator.Interfaces.ToolSpecifications
{
    public interface ISimpleCondition : ICondition
    {
        /// <summary>
        /// Dimension of the quantity, which is related to this condition
        /// </summary>
        string Dimension { get; set; }

        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meets this relation -> this is the relation
        /// </summary>
        Relations ValidIf { get; set; }

        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meet this relation  -> this is the value
        /// </summary>
        double ValidIf_Value { get; set; }

    }
}
