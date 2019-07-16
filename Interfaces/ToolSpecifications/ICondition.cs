using Interfaces.Misc;

namespace Interfaces.ToolSpecifications
{

    public interface ICondition : INamed, IXmlStorable
    {

        /// <summary>
        /// type of the calculation that is required by this condition 
        /// </summary>
        CalculationTypes CalculationType { get; }

        /// <summary>
        /// Validity of the condition -> if false, the condition is switched off
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Defines whether relative or absolute condition
        /// </summary>
        Relativity RelOrAbs { get; }
    }



    public interface ICondition<T> : ICondition
        where T : struct
    {
        /// <summary>
        /// Relation in the condition    e.g.  <  >  ==  >=  <=
        /// </summary>
        Relations ConditionRelation { get; }

        /// <summary>
        /// valueof part of the relation. The RIGHT value of the comparison
        /// </summary>
        T LeftValue { get; }

        /// <summary>
        /// Checks the condition. The calculatin result contains the approppriate result
        /// </summary>
        /// <param name="rightValue">the calculation result which will be used in the condition comparison</param>
        /// <returns>the relation is met (true) or not (false)</returns>
        bool Compare(T rightValue);
    }



    public interface IConditionHandler : ICondition
    {
        new CalculationTypes CalculationType { get; set; }

        new bool Enabled { get; set; }

        new Relativity RelOrAbs { get; set; }
    }


    public interface IConditionHandler<T> : ICondition<T>
        where T : struct
    {
        new Relations ConditionRelation { get; set; }

        new T LeftValue { get; set; }
    }

}
