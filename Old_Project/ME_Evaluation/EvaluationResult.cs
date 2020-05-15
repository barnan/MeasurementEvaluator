using DataStructures;
using Interfaces.BaseClasses;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MeasurementEvaluator.ME_Evaluation
{
    internal class EvaluationResult : ResultBase, IEvaluationResult
    {
        public IReadOnlyList<IQuantityEvaluationResult> QuantityEvaluationResults { get; }

        public ToolNames ToolName { get; }

        public string Name { get; }

        public EvaluationResult(DateTime creationTime, bool successfulCalculation, IReadOnlyList<IQuantityEvaluationResult> quantityEvaluationResults, ToolNames toolName, string name)
            : base(creationTime, successfulCalculation)
        {
            QuantityEvaluationResults = quantityEvaluationResults;
            ToolName = toolName;
            Name = name;
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


    internal class QuantityEvaluationResult : IQuantityEvaluationResult
    {
        public IReadOnlyList<IConditionEvaluationResult> ConditionEvaluationResults { get; }

        public IQuantity Quantity { get; }

        public QuantityEvaluationResult(IReadOnlyList<IConditionEvaluationResult> conditionCalculationResults, IQuantity quantity)
        {
            ConditionEvaluationResults = conditionCalculationResults;
            Quantity = quantity;
        }
    }
}
