﻿using System.Collections.Generic;

namespace Measurement_Evaluator.Interfaces.ReferenceSample
{
    public enum SampleOrientation
    {
        Orientation1 = 0,
        Orientation2 = 90,
        Orientation3 = 270,
        Orientation4 = 360
    };


    // TODO: separate reader and writer interfaces
    interface IReferenceSample
    {
        /// <summary>
        /// name or ID of the sample
        /// </summary>
        string SampleID { get; set; }

        /// <summary>
        /// reference values, which characterise the sample
        /// </summary>
        List<IReferenceValue> ListOfReferenceValues { get; set; }

        /// <summary>
        /// orientation of the sample, when its reference values were measured
        /// </summary>
        SampleOrientation SampleOrientation { get; set; }
    }
}
