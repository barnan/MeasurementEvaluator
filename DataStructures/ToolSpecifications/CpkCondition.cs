using Interfaces;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;

namespace DataStructures.ToolSpecifications
{
    public class CpkCondition : ConditionBase<double>, ICpkCondition
    {
        public double HalfTolerance { get; }
        public RELATIVEORABSOLUTE RelOrAbs { get; }

        public CpkCondition()
            : base()
        {
            HalfTolerance = 0;
            RelOrAbs = RELATIVEORABSOLUTE.ABSOLUTE;
        }


        public CpkCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, RELATIVEORABSOLUTE relativeorabsolute, double halfTolerance)
            : base(name, calculationtype, value, relation, enabled)
        {
            RelOrAbs = relativeorabsolute;
            HalfTolerance = halfTolerance;
        }


        #region IFormattable

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{base.ToString()}{Environment.NewLine}HalfTolerance: {HalfTolerance}{Environment.NewLine}{RelOrAbs}";
        }

        #endregion

        protected override bool Evaluate(ICalculationResult calculationResult)
        {
            if (!CheckCalculationType(calculationResult, CalculationType))
            {
                return false;
            }

            if (!(calculationResult is IQCellsCalculationResult qcellsResult))
            {
                return false;
            }

            return Compare(qcellsResult.Result);
        }

    }


}
