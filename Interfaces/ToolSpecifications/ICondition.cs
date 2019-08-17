using Interfaces.BaseClasses;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using System;

namespace Interfaces.ToolSpecifications
{

    public interface ICondition : INamed, IXmlStorable, IFormattable
    {

        /// <summary>
        /// type of the calculation that is required by this condition 
        /// </summary>
        CalculationTypeHandler CalculationTypeHandler { get; }


        /// <summary>
        /// Validity of the condition -> if false, the condition is switched off
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Relation in the condition    e.g.  <  >  ==  >=  <=
        /// </summary>
        RelationHandler ConditionRelation { get; }

        /// <summary>
        /// Checks the condition. The calculation result contains the approppriate result
        /// </summary>
        /// <param name="calculationResult"></param>
        /// <param name="dateTime"></param>
        /// <param name="measSerie"></param>
        /// <param name="referenceValue"></param>
        /// <returns>the relation is met (true) or not (false)</returns>
        IConditionEvaluationResult EvaluateCondition(IResult calculationResult, DateTime dateTime, IReferenceValue referenceValue);
    }



    public interface ICondition<T> : ICondition
        where T : struct
    {
        /// <summary>
        /// valueof part of the relation. The RIGHT value of the comparison
        /// </summary>
        T LeftValue { get; }
    }



    public interface IConditionHandler : ICondition
    {
        new CalculationTypeHandler CalculationTypeHandler { get; set; }

        //new Relativity Relativity { get; set; }

        new bool Enabled { get; set; }

        new RelationHandler ConditionRelation { get; set; }
    }


    public interface IConditionHandler<T> : ICondition<T>
        where T : struct
    {
        new T LeftValue { get; set; }
    }

}
