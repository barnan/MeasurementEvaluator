using System;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{
    public interface IQuantitySpecification : IComparable<IQuantitySpecification>
    {
        IQuantity Quantity { get; }

        /// <summary>
        /// Condition set which is realted to this quantity
        /// </summary>
        IReadOnlyList<ICondition> Conditions { get; }

    }


    public interface IQuantitySpecificationHandler : IQuantitySpecification
    {
        new IQuantity Quantity { get; set; }

        /// <summary>
        /// Condition set which is realted to this quantity
        /// </summary>
        new IReadOnlyList<ICondition> Conditions { get; set; }

    }

}
