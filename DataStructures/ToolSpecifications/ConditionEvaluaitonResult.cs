using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{
    internal class ConditionEvaluaitonResult : ResultBase, IConditionEvaluationResult
    {
        public IMeasurementSerie MeasurementSerieData { get; }

        public ICondition Condition { get; }

        public IReferenceValue ReferenceValue { get; }

        public bool ConditionIsMet { get; }

        public IResult CalculationResult { get; }


        public ConditionEvaluaitonResult(DateTime creationTime, IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue, bool conditionIsMet, IResult calculationResult)
            : base(creationTime, calculationResult.Successful)
        {
            MeasurementSerieData = measurementSerieData;
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

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return "";
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