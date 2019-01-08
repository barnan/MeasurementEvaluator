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


    public interface ICondition<T> : ICondition where T : struct
    {
        /// <summary>
        /// valueof part of the relation
        /// </summary>
        T Value { get; }


        /// <summary>
        /// Checks the condition. The RIGHT value and the relation is stored in the condition
        /// </summary>
        /// <param name="value">LEFT value</param>
        /// <returns></returns>
        bool Compare(T value);
    }

}
