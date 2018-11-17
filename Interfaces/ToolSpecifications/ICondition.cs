namespace Interfaces.ToolSpecifications
{

    public interface ICondition
    {

        /// <summary>
        /// Name of the condition
        /// </summary>
        string Name { get; }

        /// <summary>
        /// type of the calculation what this condition requires.
        /// </summary>
        CalculationTypes CalculationType { get; }

        /// <summary>
        /// Relation in the condition       e.g.   <   >   ==   >=   <=
        /// </summary>
        Relations ConditionRelation { get; }

        /// <summary>
        /// Validity of the condition -> if false, the condition does not work
        /// </summary>
        bool Enabled { get; }

    }
}
