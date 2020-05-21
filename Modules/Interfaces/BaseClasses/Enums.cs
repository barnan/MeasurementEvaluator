using System.ComponentModel;

namespace BaseClasses
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


    public enum CalculationType
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


    public enum Relativity
    {
        [Description("ABSOLUTE")]
        Absolute = 0,

        [Description("RELATIVE")]
        Relative = 1
    }


    //public enum FixedSampleOrientation
    //{
    //    [Description("0 degree")]
    //    Orientation_0 = 0,

    //    [Description("90 degree")]
    //    Orientation_90 = 90,

    //    [Description("180 degree")]
    //    Orientation_180 = 180,

    //    [Description("270 degree")]
    //    Orientation_270 = 270
    //}


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
        sec,

        [Description("degree")]
        deg
    }


    public enum ToolNames
    {
        Unknown = 0,
        Ttr,
        Shp,
        WsiChipping,
        WsiContamination,
        Ped,
        Mci,
        Pli,
        Upcd
    }

}
