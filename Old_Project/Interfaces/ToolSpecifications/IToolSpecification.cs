﻿using Interfaces.BaseClasses;
using Interfaces.Misc;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{

    public interface IToolSpecification : INamed, IXmlStorable
    {
        /// <summary>
        /// List of all defined specification of the quantities of the tool
        /// </summary>
        IReadOnlyList<IQuantitySpecification> QuantitySpecifications { get; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        ToolNames ToolName { get; }
    }


    public interface IToolSpecificationHandler : IToolSpecification, INamedHandler
    {
        /// <summary>
        /// List of all defined specification of the quantities of the tool
        /// </summary>
        new IReadOnlyList<IQuantitySpecification> QuantitySpecifications { get; set; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        new ToolNames ToolName { get; set; }

    }
}
