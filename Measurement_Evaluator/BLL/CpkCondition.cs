using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    public class CpkCondition : ConditionBase, ICpkCondition
    {
        public double HalfTolerance { get; set; }
        public RELATIVEORABSOLUTE RelOrAbs { get; set; }

        public CpkCondition()
            : base()
        { }

        public CpkCondition(double value, Relation relation, RELATIVEORABSOLUTE relorabs, bool valid, double halfTolerance)
            : base(value, relation, valid)
        {
            this.HalfTolerance = halfTolerance;
            this.RelOrAbs = relorabs;
        }

    }
}
