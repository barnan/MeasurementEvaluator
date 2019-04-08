using Interfaces;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;

namespace DataStructures.ToolSpecifications
{
    public class CpkCondition : ConditionBase<double>, ICpkConditionHandler
    {
        public double HalfTolerance { get; set; }


        public CpkCondition()
            : base()
        {
            HalfTolerance = 0;
        }


        public CpkCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, RELATIVEORABSOLUTE relativeorabsolute, double halfTolerance)
            : base(name, calculationtype, value, relation, enabled, relativeorabsolute)
        {
            HalfTolerance = halfTolerance;
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}HalfTolerance: {HalfTolerance}{Environment.NewLine}{RelOrAbs}";
        }

        #endregion

        #region protected

        protected override bool EvaluateCondition(ICalculationResult calculationResult)
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

        #endregion

    }

}
