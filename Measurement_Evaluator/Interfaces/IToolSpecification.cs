using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    interface IToolSpecification
    {
        /// <summary>
        /// List of all defined specification of the quantities
        /// </summary>
        List<IQuantitySpecification> SpecificationList { get; set; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        string ToolName { set; get; }

    }
}
