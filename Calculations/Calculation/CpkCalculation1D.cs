using Interfaces;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    internal class CpkCalculation1D : CpCalculation1D
    {
        internal CpkCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.Cpk;


        protected override IResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            if (!(condition is ICpkCondition cpkCondition))
            {
                throw new ArgumentNullException($"No {nameof(ICpkCondition)} condition received for {CalculationType} settings creation.");
            }

            if (condition.CalculationType != CalculationType)
            {
                throw new ArgumentException($"The current calculation (type: {CalculationType}) can not run with the received condition {condition.CalculationType}");
            }

            // CpkCalculation changes into CpCalculation
            if (referenceValue == null)
            {
                return base.InternalCalculation(measurementSerieData, condition, referenceValue);
            }

            if (!(referenceValue is IReferenceValue<double> doubleReferenceValue))
            {
                throw new ArgumentException($"The received reference value is not {nameof(IReferenceValue<double>)}");
            }

            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);
            double std = GetStandardDeviation(validElementList);
            double usl = doubleReferenceValue.Value + cpkCondition.HalfTolerance;
            double lsl = doubleReferenceValue.Value - cpkCondition.HalfTolerance;
            double cpk = Math.Min((average - usl) / (3 * std), (lsl - average) / (3 * std));

            _parameters.Logger.MethodTrace($"{nameof(StdCalculation1D)}: Calculated  Cp: {cpk}, USL: {usl}, LSL: {lsl}.");

            DateTime endTime = _parameters.DateTimeProvider.GetDateTime();

            return new QCellsCalculationResult(cpk,
                                               usl,
                                               lsl,
                                               endTime,
                                               true);
        }
    }
}
