using System.ComponentModel;

namespace BaseClasses.MeasurementEvaluator
{
    public enum Relations
    {
        EQUAL = 0,
        NOTEQUAL = 1,
        LESS = 2,
        LESSOREQUAL = 3,
        GREATER = 4,
        GREATEROREQUAL = 5,
    }

    public enum CalculationTypes
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("Avg")]
        Average = 1,

        [Description("Std")]
        StandardDeviation = 2,

        [Description("Cp")]
        Cp = 3,

        [Description("Cpk")]
        Cpk = 4,

        [Description("GRAndR")]
        GRAndR = 5
    }

    public enum Relativities
    {
        [Description("ABSOLUTE")]
        Absolute = 0,

        [Description("RELATIVE")]
        Relative = 1
    }

    public enum SampleOrientations
    {
        [Description("0 degree")]
        Orientation_0 = 0,

        [Description("90 degree")]
        Orientation_90 = 90,

        [Description("180 degree")]
        Orientation_180 = 180,

        [Description("270 degree")]
        Orientation_270 = 270
    }

    public enum Units
    {
        [Description("ADU")]
        ADU = 0,

        [Description("count")]
        count,

        [Description("mm")]
        mm,

        [Description("um")]
        um,

        [Description("Ohmcm")]
        Ohmcm,

        [Description("sec")]
        sec
    }

    public enum ToolNames
    {
        Unknown,
        [Description("Thickness-Topology-Resistivity")]
        TTR,
        [Description("Shape")]
        SHP,
        [Description("Wafer-Surface-Inspection-Chipping")]
        WSIChipping,
        [Description("Wafer-Surface-Inspection-Contamination")]
        WSIContamination,
        [Description("Perpendicular-Edge-Detection")]
        PED,
        [Description("Micro-Crack-Inspection")]
        MCI,
        [Description("Photolumi-Inspection")]
        PLI,
        [Description("Microwave-Detected-PhotoConductance-Decay")]
        UPCD
    }

}
