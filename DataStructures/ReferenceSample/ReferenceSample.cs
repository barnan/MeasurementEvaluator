using Interfaces;
using Interfaces.ReferenceSample;
using Miscellaneous;
using NLog;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DataStructures.ReferenceSample
{
    public class ReferenceSample : IReferenceSampleHandler
    {
        private readonly ILogger _logger;

        #region INamed

        public string Name { get; set; }

        #endregion

        private List<IReferenceValue> _referenceValues;
        public IReadOnlyList<IReferenceValue> ReferenceValues
        {
            get { return _referenceValues.AsReadOnly(); }
            set { _referenceValues = (List<IReferenceValue>)value; }
        }


        public SampleOrientation SampleOrientation { get; set; }


        public ReferenceSample()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public ReferenceSample(string name, IReadOnlyList<IReferenceValue> referenceValues)
        {
            Name = name;
            ReferenceValues = referenceValues;
        }


        #region IComparable

        public int CompareTo(IReferenceSample other)
        {
            try
            {
                if (ReferenceEquals(this, other))
                {
                    return 0;
                }

                if (ReferenceEquals(null, other))
                {
                    return 1;
                }


                if (other.Name == null)
                {
                    _logger.Error("Sample ID is null in Arrived data.");
                    return 0;
                }

                string toolName1 = Name;
                string toolName2 = other.Name;
                int toolNameComparisonResult = string.Compare(toolName1, toolName2, StringComparison.OrdinalIgnoreCase);
                if (toolNameComparisonResult != 0)
                {
                    return toolNameComparisonResult;
                }

                if (ReferenceValues.Count != other.ReferenceValues.Count)
                {
                    return ReferenceValues.Count > other.ReferenceValues.Count ? 1 : -1;
                }

                int summ = 0;
                for (int i = 0; i < ReferenceValues.Count; i++)
                {
                    summ += ReferenceValues[i].CompareTo(other.ReferenceValues[i]);
                }

                if (summ != 0)
                {
                    summ /= Math.Abs(summ);
                }

                return summ;
            }
            catch (Exception ex)
            {
                _logger.MethodError($"Exception occured: {ex}");
                return 0;
            }
        }

        public XElement SaveToXml(XElement inputElement)
        {
            this.TrySave(Name, inputElement, nameof(Name));
            this.TrySave(ReferenceValues, inputElement, nameof(ReferenceValues));
            this.TrySave(SampleOrientation, inputElement, nameof(SampleOrientation));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            this.TryLoad(inputElement, nameof(Name));
            this.TryLoad(inputElement, nameof(ReferenceValues));
            this.TryLoad(inputElement, nameof(SampleOrientation));
            return true;
        }

        #endregion
    }
}
