﻿using Interfaces.Result;
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
        /// Validity of the condition -> if false, the condition does not work
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Checks the condition. The calculatin result contains the approppriate result
        /// </summary>
        /// <param name="calculationResult">the calculation result which will be used in the condition comparison</param>
        /// <returns></returns>
        bool Compare(ICalculationResult calculationResult);
    }


    public interface ICondition<T> : ICondition where T : struct
    {

        /// <summary>
        /// Relation in the condition       e.g.   <   >   ==   >=   <=
        /// </summary>
        Relations ConditionRelation { get; }

        /// <summary>
        /// valueof part of the relation. The RIGHT value of the comparison
        /// </summary>
        T Value { get; }
    }

}
