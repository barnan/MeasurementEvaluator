using System;

namespace Interfaces.ToolSpecifications
{

    public interface ICondition : IFormattable
    {

        /// <summary>
        /// Name of the condition
        /// </summary>
        string Name { get; }

        /// <summary>
        /// type of the calculation that is required by this condition 
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
        /// valueof part of the relation. The RIGHT value of the comparison
        /// </summary>
        T Value { get; }


        /// <summary>
        /// Checks the condition. The RIGHT value and the relation is stored in the condition
        /// </summary>
        /// <param name="value">LEFT value of the comparison</param>
        /// <returns></returns>
        bool Compare(T value);
    }

}
