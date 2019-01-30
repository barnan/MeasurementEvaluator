﻿using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Calculations.Evaluation
{
    internal class EvaluationResult : IEvaluationResult
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public bool SuccessfulCalculation { get; }

        public IReadOnlyList<IQuantityEvaluationResult> EvaluationResults { get; }


        public EvaluationResult(DateTime startTime, DateTime endTime, bool successfulCalculation, IReadOnlyList<IQuantityEvaluationResult> evaluationResults)
        {
            StartTime = startTime;
            EndTime = endTime;
            SuccessfulCalculation = successfulCalculation;
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


    internal class ConditionEvaluaitonResult : IConditionEvaluationResult
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public bool SuccessfulCalculation { get; }

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

        public ICalculationResult Result { get; }


        public ConditionEvaluaitonResult(DateTime startTime, DateTime endTime, bool successfulCalculation, IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue, bool conditionIsMet, ICalculationResult result)
        {
            StartTime = startTime;
            EndTime = endTime;
            SuccessfulCalculation = successfulCalculation;
            MeasurementSerieData = measurementSerieData;
            Condition = condition;
            ReferenceValue = referenceValue;
            ConditionIsMet = conditionIsMet;
            Result = result;
        }

    }


}