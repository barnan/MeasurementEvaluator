using System.Collections.Generic;

namespace Interfaces.ReferenceSample
{

    // TODO: separate reader and writer interfaces
    public interface IReferenceSample
    {
        /// <summary>
        /// name or ID of the sample
        /// </summary>
        string SampleID { get; set; }

        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        List<IReferenceValue> ReferenceValues { get; set; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        SampleOrientation SampleOrientation { get; set; }
    }
}
