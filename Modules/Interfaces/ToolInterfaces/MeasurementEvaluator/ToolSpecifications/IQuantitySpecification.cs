using System.Collections.Generic;

namespace ToolSpecificInterfaces.MeasurementEvaluator.ToolSpecifications
{
    public interface IQuantitySpecification
    {
        /// <summary>
        /// the quantity which the condition is related
        /// </summary>
        IQuantity Quantity { get; }

        /// <summary>
        /// Condition set which is realted to the quantity
        /// </summary>
        List<ICondition> Conditions { get; }

    }


    public interface IQuantitySpecificationHandler : IQuantitySpecification
    {
        new IQuantity Quantity { get; set; }

        new List<ICondition> Conditions { get; set; }

    }

}
