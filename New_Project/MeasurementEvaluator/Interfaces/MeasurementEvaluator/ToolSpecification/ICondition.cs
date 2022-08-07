using BaseClasses.MeasurementEvaluator;
using Interfaces.Misc;

namespace Interfaces.MeasurementEvaluator.ToolSpecification
{
    public interface ICondition : INamed, IXmlStorable, IFormattable
    {
        /// <summary>
        /// Validity of the condition -> if false, the condition is switched off
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// type of the calculation that is required by this condition 
        /// </summary>
        CalculationTypes CalculationType { get; }

        /// <summary>
        /// relative or absolute
        /// </summary>
        Relativities Relativity { get; }

        /// <summary>
        /// Relation in the condition    e.g.  <  >  ==  >=  <=
        /// </summary>
        Relations Relation { get; }
    }


    //public interface ICondition<T> : ICondition
    //    where T : struct
    //{
    //    /// <summary>
    //    /// value of part of the relation. The RIGHT value of the comparison
    //    /// </summary>
    //    T LeftValue { get; }
    //}


    public interface IConditionHandler : ICondition, INamedHandler
    {
        new bool IsEnabled { get; set; }

        new CalculationTypes CalculationType { get; set; }

        new Relativities Relativity { get; set; }

        new Relations Relation { get; set; }
    }


    //public interface IConditionHandler<T> : ICondition<T>
    //    where T : struct
    //{
    //    new T LeftValue { get; set; }
    //}
}
