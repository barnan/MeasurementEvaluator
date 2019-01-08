﻿using Interfaces;
using Interfaces.ToolSpecifications;

namespace DataStructures.ToolSpecifications
{
    public partial class CpkCondition : ConditionBase<double>, ICpkCondition
    {
        public double HalfTolerance { get; set; }
        public RELATIVEORABSOLUTE RelOrAbs { get; set; }

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

    }


}
