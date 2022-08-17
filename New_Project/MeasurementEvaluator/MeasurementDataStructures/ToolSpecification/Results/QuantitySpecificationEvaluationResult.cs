using System.Xml.Linq;
using Interfaces.MeasurementEvaluator.Result;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification.Results
{
    internal class QuantitySpecificationEvaluationResult : IQuantitySpecificationEvaluationResult
    {
        private List<IConditionEvaluationResult> _conditionEvaluationResults;

        public IReadOnlyList<IConditionEvaluationResult> ConditionEvaluationResults => _conditionEvaluationResults.AsReadOnly();

        public IQuantity Quantity { get; }

        public XElement SaveToXml(XElement inputElement)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromXml(XElement inputElement)
        {
            throw new NotImplementedException();
        }

        public DateTime CreationTime { get; }

        public bool IsSuccessful { get; }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}
