using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{

    public interface IToolSpecification : IFormattable, IStoredDataOnHDD, IComparable<IToolSpecification>
    {
        /// <summary>
        /// List of all defined specification of the quantities of the tool
        /// </summary>
        IReadOnlyList<IQuantitySpecification> Specifications { get; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        ToolNames ToolName { get; }

    }


    public interface IToolSpecificationOnHDDHandler : IToolSpecification, IStoredDataOnHDDHandler
    {
        /// <summary>
        /// List of all defined specification of the quantities of the tool
        /// </summary>
        IReadOnlyList<IQuantitySpecification> Specifications { get; set; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        ToolNames ToolName { get; set; }

    }
}
