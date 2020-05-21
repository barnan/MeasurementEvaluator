using BaseClasses;
using Interfaces.Misc;
using System;

namespace ToolSpecificInterfaces.MeasurementEvaluator.ToolSpecifications
{

    public interface ICondition : INamed, IFormattable
    {
        /// <summary>
        /// Type of the required calculation
        /// </summary>
        CalculationType CalculationType { get; }

        /// <summary>
        /// Validity of the condition -> if false, the condition is switched off
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// relation 
        /// </summary>
        Relations Relation { get; }
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
        new CalculationType CalculationType { get; set; }

        new bool Enabled { get; set; }

        new Relations Relation { get; set; }
    }


    public interface IConditionHandler<T> : ICondition<T>
        where T : struct
    {
        new T LeftValue { get; set; }
    }

}
