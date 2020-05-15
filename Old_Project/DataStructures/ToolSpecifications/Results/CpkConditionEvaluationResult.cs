using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Globalization;

namespace DataStructures.ToolSpecifications.Results
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
