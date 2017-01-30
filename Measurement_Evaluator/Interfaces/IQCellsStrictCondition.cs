using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public interface ICpkCondition  :IConditionBase
    {
        /// <summary>
        /// HalfTolerance
        /// </summary>
        double HalfTolerance { get; set; }

        /// <summary>
        /// Defines whether relaite or absolute condition
        /// </summary>
        RELATIVEORABSOLUTE RelOrAbs { get; set; }
    }
}
