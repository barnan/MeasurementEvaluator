using Interfaces;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{
    public class CpkCondition : ConditionBase<double>, ICpkCondition<double>
    {
        public double HalfTolerance { get; set; }
        public RELATIVEORABSOLUTE RelOrAbs { get; set; }

        public CpkCondition()
            : base()
        {
            HalfTolerance = 0;
            RelOrAbs = RELATIVEORABSOLUTE.ABSOLUTE;
        }


        public CpkCondition(double value, Relations relation, bool valid, IComparer<double> comparer, RELATIVEORABSOLUTE relativeorabsolute, double halfTolerance)
            : base(value, relation, valid, comparer)
        {
            RelOrAbs = relativeorabsolute;
            HalfTolerance = halfTolerance;
        }

    }
}
