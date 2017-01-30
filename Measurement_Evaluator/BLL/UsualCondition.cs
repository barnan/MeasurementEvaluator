using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    public class UsualCondition : ConditionBase , IUsualCondition
    {
        public string Dimension { get; set; }
        public Relation ValidIfRelation_Relation { get; set; }
        public double ValidIfRelation_Value { get; set; }


        public UsualCondition()
            : base()
        {
        }

        public UsualCondition(double value, string dim, Relation relation, bool valid, Relation workwhenrel, double workwhenvalue)
            : base(value, relation, valid)
        {
            this.Dimension = dim;
            this.ValidIfRelation_Relation = workwhenrel;
            this.ValidIfRelation_Value = workwhenvalue;
        }


        public override string ToString()
        {
            if (Valid)
                return base.ToString() + " " + Dimension + ". The second condition is valid, when Value is "
                    + ValidIfRelation_Relation.ToString() + " than " + ValidIfRelation_Value.ToString();
            else
                return base.ToString() + " " + Dimension + ". The second condition is valid, when Value is "
                    + ValidIfRelation_Relation.ToString() + " than " + ValidIfRelation_Value.ToString();
        }

    }

}
