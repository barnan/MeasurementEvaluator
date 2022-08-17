using System.Xml.Linq;
using Interfaces;
using Interfaces.MeasurementEvaluator.Result;

namespace MeasurementDataStructures.ToolSpecification.Results
{
    internal class ToolSpecificationEvaluationResult : IToolSpecificationEvaluationResult
    {
        private List<IQuantitySpecificationEvaluationResult> _quantityEvaluationResults;

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

        public string Name { get; }

        public IReadOnlyList<IQuantitySpecificationEvaluationResult> QuantityEvaluationResults => _quantityEvaluationResults.AsReadOnly();

        public ToolNames ToolName { get; }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}
