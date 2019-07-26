using DataStructures;
using Interfaces.Result;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MeasurementEvaluator.ME_Evaluation
{
    internal class EvaluationResult : ResultBase, IEvaluationResult
    {
        public IReadOnlyList<IQuantityEvaluationResult> QuantityEvaluationResults { get; }


        public EvaluationResult(DateTime creationTime, bool successfulCalculation, IReadOnlyList<IQuantityEvaluationResult> quantityEvaluationResults)
            : base(creationTime, successfulCalculation)
        {
            QuantityEvaluationResults = quantityEvaluationResults;
        }

        public override XElement SaveToXml(XElement input)
        {
            throw new NotImplementedException();
        }

        public override bool LoadFromXml(XElement input)
        {
            throw new NotImplementedException();
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return "";
        }
    }


    internal class QuantityEvaluationResult : IQuantityEvaluationResult
    {
        public IReadOnlyList<IConditionEvaluationResult> ConditionEvaluationResults { get; }


        public QuantityEvaluationResult(IReadOnlyList<IConditionEvaluationResult> conditionCalculationResults)
        {
            ConditionEvaluationResults = conditionCalculationResults;
        }
    }
}
