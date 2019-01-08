using System;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{
    public interface IQuantitySpecification : IFormattable
    {
        /// <summary>
        /// Name of this quantity
        /// </summary>
        string QuantityName { get; }

        /// <summary>
        /// unit of quantity
        /// </summary>
        Units Dimension { get; }

        /// <summary>
        /// Condition set which is realted to this quantity
        /// </summary>
        IReadOnlyList<ICondition> Conditions { get; }

    }
}
