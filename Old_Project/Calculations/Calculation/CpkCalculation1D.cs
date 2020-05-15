using Interfaces.BaseClasses;
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

        public override CalculationType CalculationType => CalculationType.Cpk;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            if (!(condition is ICpkCondition cpkCondition))
            {
                throw new ArgumentNullException($"No {nameof(ICpkCondition)} condition received for {CalculationType} settings creation.");
            }

            // CpkCalculation changes into CpCalculation
            if (referenceValue == null)
            {
                return base.InternalCalculation(measurementSerieData, condition, null);
            }

            if (!(referenceValue is IReferenceValue<double> doubleReferenceValue))
            {
                throw new ArgumentException($"The received reference value is not {nameof(IReferenceValue<double>)}");
            }

            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);
            double std = GetStandardDeviation(validElementList, average);
            double usl = doubleReferenceValue.Value + cpkCondition.HalfTolerance;
            double lsl = doubleReferenceValue.Value - cpkCondition.HalfTolerance;
            double cpk = Math.Min((average - usl) / (3 * std), (lsl - average) / (3 * std));

            _parameters.Logger.LogMethodTrace($"{nameof(StdCalculation1D)}: Calculated  Cp: {cpk}, USL: {usl}, LSL: {lsl}, average:{average}");

            return new QCellsCalculationResult(_parameters.DateTimeProvider.GetDateTime(),
                                                true,
                                                measurementSerieData,
                                                cpk,
                                                usl,
                                                lsl,
                                                average);
        }
    }
}
