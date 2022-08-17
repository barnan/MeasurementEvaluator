using System.Globalization;
using Interfaces.MeasurementEvaluator.ReferenceSample;
using Interfaces.MeasurementEvaluator.Result;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification.Results
{
    internal class CpkConditionEvaluationResult : ConditionEvaluationResultBase
    {
        public CpkConditionEvaluationResult(DateTime creationTime, ICondition condition, IReferenceValue referenceValue, bool conditionIsMet, ICalculationResult calculationResult)
            : base(creationTime, condition, referenceValue, conditionIsMet, calculationResult)
        { }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            switch (format.ToUpperInvariant())
            {
                case "G":
                    return ToString();
                case "GRID":
                    return $"{Condition.ToString(format, formatProvider)}";           // todo: kijavítani!!!!!
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }
    }
}
