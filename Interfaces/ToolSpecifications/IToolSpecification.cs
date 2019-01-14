using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{

    public interface IToolSpecification : IFormattable, IStoredData, IComparer<IToolSpecification>
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


    public interface IToolSpecificationHandler : IToolSpecification, IStoredDataHandler
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
