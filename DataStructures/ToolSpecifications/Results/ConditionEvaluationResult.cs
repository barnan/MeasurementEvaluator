using Interfaces.BaseClasses;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications.Results
{
    internal class ConditionEvaluationResult : ResultBase, IConditionEvaluationResult
    {
        public ICondition Condition { get; }

        public IReferenceValue ReferenceValue { get; }

        public bool ConditionIsMet { get; }

        public ICalculationResult CalculationResult { get; }


        public ConditionEvaluationResult(DateTime creationTime, ICondition condition, IReferenceValue referenceValue, bool conditionIsMet, ICalculationResult calculationResult)
            : base(creationTime, calculationResult.Successful)
        {
            Condition = condition;
            ReferenceValue = referenceValue;
            ConditionIsMet = conditionIsMet;
            CalculationResult = calculationResult;
        }


        public override string ToString()
        {
            if (ReferenceValue == null || Condition == null)
            {
                return string.Empty;
            }

            return base.ToString() + Condition + ReferenceValue + CalculationResult + $"Condition is met: {ConditionIsMet}{Environment.NewLine}";

        }

        public virtual string ToString(string format, IFormatProvider formatProvider)
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
                    return $"{(Condition.RelOrAbs == Relativity.Absolute ? "(abs)" : "(rel)")} {Condition.ToString(format, formatProvider)}";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }

        public override XElement SaveToXml(XElement input)
        {
            throw new NotImplementedException();
        }

        public override bool LoadFromXml(XElement input)
        {
            throw new NotImplementedException();
        }
    }
}