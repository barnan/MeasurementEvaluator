using System.Xml.Linq;
using Interfaces;
using Interfaces.MeasurementEvaluator.ReferenceSample;

namespace MeasurementDataStructures.Reference
{
    public class ReferenceSample : IReferenceSampleHandler
    {
        #region INamed

        public string Name { get; set; }

        #endregion

        private List<IReferenceValue> _referenceValues;
        public IReadOnlyList<IReferenceValue> ReferenceValues
        {
            get => _referenceValues.AsReadOnly();
            set => _referenceValues = (List<IReferenceValue>)value;
        }

        public SampleOrientations SampleOrientation { get; set; }

        public ReferenceSample(string name, IReadOnlyList<IReferenceValue> referenceValues)
        {
            Name = name;
            ReferenceValues = referenceValues;
        }

        #region IComparable

        public int CompareTo(IReferenceSample other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other?.Name))
            {
                return 1;
            }

            string toolName1 = Name;
            string toolName2 = other.Name;
            int toolNameComparisonResult = string.Compare(toolName1, toolName2, StringComparison.OrdinalIgnoreCase);
            if (toolNameComparisonResult != 0)
            {
                return toolNameComparisonResult;
            }

            return ReferenceValues.Count - other.ReferenceValues.Count;
        }

        public XElement SaveToXml(XElement inputElement)
        {
            //this.TrySave(Name, inputElement, nameof(Name));
            //this.TrySave(ReferenceValues, inputElement, nameof(ReferenceValues));
            //this.TrySave(SampleOrientation, inputElement, nameof(SampleOrientation));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            //this.TryLoad(inputElement, nameof(Name));
            //this.TryLoad(inputElement, nameof(ReferenceValues));
            //this.TryLoad(inputElement, nameof(SampleOrientation));
            return true;
        }

        #endregion
    }
}
