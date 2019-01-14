using Interfaces;
using Interfaces.ReferenceSample;
using System.Collections.Generic;
using IReferenceValue = Interfaces.ReferenceSample.IReferenceValue;

namespace DataAcquisition
{
    //[Serializable]
    public class ReferenceSample : IReferenceSample
    {
        private readonly ReferenceSampleOnHDD _referenceSampleOnHDD;


        public string FullNameOnStorage { get; }


        public string SampleID { get; }


        public IReadOnlyList<IReferenceValue> ReferenceValues => _referenceSampleOnHDD.ReferenceValues;


        public SampleOrientation SampleOrientation { get; }


        public ReferenceSample(string name, ReferenceSampleOnHDD refsampl, SampleOrientation orientation)
        {
            SampleID = name;
            SampleOrientation = orientation;

            _referenceSampleOnHDD = refsampl;
        }


        public int Compare(IReferenceSample x, IReferenceSample y)
        {
            throw new System.NotImplementedException();
        }
    }
}
