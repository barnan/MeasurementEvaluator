using System;
using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{

    public interface IToolSpecification : IFormattable
    {
        /// <summary>
        /// List of all defined specification of the quantities of the tool
        /// </summary>
        List<IQuantitySpecification> Specifications { get; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        ToolNames ToolName { get; }

        /// <summary>
        /// full path and name of the file which contains this specification
        /// </summary>
        string FileFullPathAndName { get; }
    }

}
