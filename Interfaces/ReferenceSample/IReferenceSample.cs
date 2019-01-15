using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace Interfaces.ReferenceSample
{

    public interface IReferenceSample : IComparable<IReferenceSample>, IStoredDataOnHDD
    {
        /// <summary>
        /// name or ID of the sample
        /// </summary>
        string SampleID { get; }

        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        IReadOnlyList<IReferenceValue> ReferenceValues { get; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        SampleOrientation SampleOrientation { get; }

    }



    public interface IReferenceSampleOnHDDHandler : IReferenceSample, IStoredDataOnHDDHandler
    {
        /// <summary>
        /// name or ID of the sample
        /// </summary>
        string SampleID { get; set; }

        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        IReadOnlyList<IReferenceValue> ReferenceValues { get; set; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        SampleOrientation SampleOrientation { get; set; }

    }


}
