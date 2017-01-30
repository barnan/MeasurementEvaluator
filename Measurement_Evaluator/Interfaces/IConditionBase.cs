using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public enum Relation { EQUAL = 0, NOTEQUAL, LESS, LESSOREQUAL, GREATER, GREATEROREQUAL }
    public enum RELATIVEORABSOLUTE { ABSOLUTE = 0, RELATIVE = 1 }

    public interface IConditionBase
    {
        /// <summary>
        /// value of the condition
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// Relation in the condition       e.g.   <   >   ==   >=   <=
        /// </summary>
        Relation ConditionRel { get; set; }

        /// <summary>
        /// Validity of the condition -> if false, the condition does not work
        /// </summary>
        bool Valid { get; set; }


    }
}
