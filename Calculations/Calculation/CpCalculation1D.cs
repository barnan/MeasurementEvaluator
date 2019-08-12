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
    internal class CpCalculation1D : CalculationBase
    {
        internal CpCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypesValues CalculationType => CalculationTypesValues.Cp;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            if (!(condition is ICpkCondition cpkCondition))
            {
                _parameters.Logger.Error($"No {nameof(cpkCondition)} condition received for settings creation.");
                return null;
            }

            if (condition.CalculationType.CalculationTypeValue != CalculationType)
            {
                throw new ArgumentException($"The current calculation (type: {CalculationType}) can not run with the received condition {condition.CalculationType}");
            }

            if (!(referenceValue is IReferenceValue<double>))
            {
                throw new ArgumentException($"The received reference value is not {nameof(IReferenceValue<double>)}");
            }

            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);
            double std = GetStandardDeviation(validElementList, average);
            double usl = average + cpkCondition.HalfTolerance;
            double lsl = average - cpkCondition.HalfTolerance;
            double cp = (usl - lsl) / (6 * std);

            _parameters.Logger.LogMethodTrace($"{nameof(StdCalculation1D)}: Calculated  Cp: {cp}, USL: {usl}, LSL: {lsl}, average: {average}");

            return new QCellsCalculationResult(_parameters.DateTimeProvider.GetDateTime(),
                                                true,
                                                measurementSerieData,
                                                cp,
                                                usl,
                                                lsl,
                                                average);
        }
    }
}
