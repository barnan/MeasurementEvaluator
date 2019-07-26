﻿using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using System;

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

        /// <summary>
        /// Checks the condition. The calculation result contains the approppriate result
        /// </summary>
        /// <param name="calculationResult"></param>
        /// <param name="dateTime"></param>
        /// <param name="measSerie"></param>
        /// <param name="referenceValue"></param>
        /// <returns>the relation is met (true) or not (false)</returns>
        IConditionEvaluationResult Compare(IResult calculationResult, DateTime dateTime, IMeasurementSerie measSerie, IReferenceValue referenceValue);
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
    }



    public interface IConditionHandler : ICondition, IFormattable
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
