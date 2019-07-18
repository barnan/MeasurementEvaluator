using DataStructures;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MeasurementEvaluator.ME_Evaluation
{
    internal class EvaluationResult : ResultBase, IEvaluationResult
    {
        public IReadOnlyList<IQuantityEvaluationResult> EvaluationResults { get; }


        public EvaluationResult(DateTime creationTime, bool successfulCalculation, IReadOnlyList<IQuantityEvaluationResult> evaluationResults)
            : base(creationTime, successfulCalculation)
        {
            EvaluationResults = evaluationResults;
        }

    }


    internal class QuantityEvaluationResult : IQuantityEvaluationResult
    {
        public IReadOnlyList<IConditionEvaluationResult> ConditionCalculationResults { get; }


        public QuantityEvaluationResult(IReadOnlyList<IConditionEvaluationResult> conditionCalculationResults)
        {
            ConditionCalculationResults = conditionCalculationResults;
        }
    }


    internal class ConditionEvaluaitonResult : ResultBase, IConditionEvaluationResult
    {
        public XElement Save()
        {
            throw new NotImplementedException();
        }

        public bool Load(XElement input)
        {
            throw new NotImplementedException();
        }

        public IMeasurementSerie MeasurementSerieData { get; }

        public ICondition Condition { get; }

        public IReferenceValue ReferenceValue { get; }

        public bool ConditionIsMet { get; }

        public IResult Result { get; }


        public ConditionEvaluaitonResult(DateTime creationTime, bool successfulCalculation, IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue, bool conditionIsMet, IResult result)
            : base(creationTime, successfulCalculation)
        {
            MeasurementSerieData = measurementSerieData;
            Condition = condition;
            ReferenceValue = referenceValue;
            ConditionIsMet = conditionIsMet;
            Result = result;
        }

    }


}
