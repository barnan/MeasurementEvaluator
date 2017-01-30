using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public interface IUsualCondition :IConditionBase
    {
        /// <summary>
        /// Dimension of the quantity, which is related to this condition
        /// </summary>
        string Dimension { get; set; }

        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meet this relation -> this is the relation
        /// </summary>
        Relation ValidIfRelation_Relation { get; set; }

        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meet this relation  -> this is the value
        /// </summary>
        double ValidIfRelation_Value { get; set; }

    }
}
