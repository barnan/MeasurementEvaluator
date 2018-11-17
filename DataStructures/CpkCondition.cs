using Interfaces;
using Interfaces.ToolSpecifications;

namespace Measurement_Evaluator.BLL
{
    public class CpkCondition : ConditionBase, ICpkCondition
    {
        public double HalfTolerance { get; set; }
        public RELATIVEORABSOLUTE RelOrAbs { get; set; }

        public CpkCondition()
            : base()
        { }


        public CpkCondition(double value, Relations relation, RELATIVEORABSOLUTE relativeorabsolute, bool valid, double halfTolerance)
            : base(value, relation, valid)
        {
            this.HalfTolerance = halfTolerance;
            this.RelOrAbs = relativeorabsolute;
        }

    }
}
