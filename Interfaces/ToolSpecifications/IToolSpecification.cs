using System.Collections.Generic;

namespace Measurement_Evaluator.Interfaces.ToolSpecifications
{

    public enum ToolNames : byte
    {
        Unknown = 0,
        TTR,
        SHP,
        WSIChipping,
        WSIContamination,
        PED,
        MCI,
        PLI,
        UPCD
    }



    interface IToolSpecification
    {
        /// <summary>
        /// List of all defined specification of the quantities of the tool
        /// </summary>
        List<IQuantitySpecification> SpecificationList { get; set; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        ToolNames ToolName { set; get; }

    }
}
