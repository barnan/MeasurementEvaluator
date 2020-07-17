using BaseClasses;
using Interfaces.Misc;
using System.Collections.Generic;

namespace ToolSpecificInterfaces.MeasurementEvaluator.ReferenceSample
{

    public interface IReferenceSample : INamed   //, IComparable<IGenericReferenceSample>
    {
        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        List<IGenericReferenceValue> ReferenceValues { get; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        SampleOrientation SampleOrientation { get; }
    }


    public interface IReferenceSampleHandler : IReferenceSample, INamedHandler
    {
        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        new List<IGenericReferenceValue> ReferenceValues { get; set; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        new SampleOrientation SampleOrientation { get; set; }
    }

}
