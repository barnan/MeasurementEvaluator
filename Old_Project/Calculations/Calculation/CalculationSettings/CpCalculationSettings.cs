﻿using Interfaces;
using Interfaces.BaseClasses;
using Interfaces.Calculation;

namespace Calculations.Calculation.CalculationSettings
{
    internal class CpCalculationSettings : CalculationSettingsBase, ICpCalculationSettings
    {
        public CpCalculationSettings(CalculationType calculationType, double halfTolerance)
            : base(calculationType)
        {
            HalfTolerance = halfTolerance;
        }

        public double HalfTolerance { get; }
    }
}
