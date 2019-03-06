using System;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{
    public interface IQuantitySpecification : IComparable<IQuantitySpecification>
    {
        /// <summary>
        /// the quantity which the condition is related
        /// </summary>
        IQuantity Quantity { get; }

        /// <summary>
        /// Condition set which is realted to the quantity
        /// </summary>
        IReadOnlyList<ICondition> Conditions { get; }

    }


    public interface IQuantitySpecificationHandler : IQuantitySpecification
    {
        new IQuantity Quantity { get; set; }

        new IReadOnlyList<ICondition> Conditions { get; set; }

    }

}
