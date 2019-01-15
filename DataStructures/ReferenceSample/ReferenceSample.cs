using DataStructures.ReferenceSample;
using Interfaces;
using Interfaces.ReferenceSample;
using Miscellaneous;
using NLog;
using System;
using System.Collections.Generic;
using IReferenceValue = Interfaces.ReferenceSample.IReferenceValue;

namespace DataAcquisition
{
    //[Serializable]
    public class ReferenceSample : IReferenceSample
    {
        private readonly ReferenceSampleOnHDD _referenceSampleOnHDD;
        private readonly ILogger _logger;


        public string FullNameOnHDD { get; }


        public string SampleID { get; }


        public IReadOnlyList<IReferenceValue> ReferenceValues => _referenceSampleOnHDD.ReferenceValues;


        public SampleOrientation SampleOrientation { get; }


        public ReferenceSample(string name, ReferenceSampleOnHDD refsampl, SampleOrientation orientation)
        {
            SampleID = name;
            SampleOrientation = orientation;

            _referenceSampleOnHDD = refsampl;

            _logger.MethodInfo($"{SampleID} reference sample created.");
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


                if (other.SampleID == null)
                {
                    _logger.Error("Sample ID is null in Arrived data.");
                    return 0;
                }

                string toolName1 = SampleID;
                string toolName2 = other.SampleID;

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

        #endregion
    }
}
