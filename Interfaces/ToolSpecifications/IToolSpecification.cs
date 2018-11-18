using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{

    public interface IToolSpecification
    {
        /// <summary>
        /// List of all defined specification of the quantities of the tool
        /// </summary>
        List<IQuantitySpecification> Specifications { get; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        ToolNames ToolName { set; }
    }

}
