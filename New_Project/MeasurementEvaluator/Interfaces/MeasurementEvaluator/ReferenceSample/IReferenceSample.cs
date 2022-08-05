using BaseClasses.MeasurementEvaluator;
using Interfaces.Misc;

namespace Interfaces.MeasurementEvaluator.ReferenceSample
{
    public interface IReferenceSample : IComparable<IReferenceSample>, INamed, IXmlStorable
    {
        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        IReadOnlyList<IReferenceValue> ReferenceValues { get; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        SampleOrientations SampleOrientation { get; }
    }



    public interface IReferenceSampleHandler : IReferenceSample, INamedHandler
    {
        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        new IReadOnlyList<IReferenceValue> ReferenceValues { get; set; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        new SampleOrientations SampleOrientation { get; set; }
    }

}