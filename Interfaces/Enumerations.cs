using System.ComponentModel;

namespace Interfaces
{
    public enum Relations : int
    {
        EQUAL = 0,
        NOTEQUAL = 1,
        LESS = 2,
        LESSOREQUAL = 3,
        GREATER = 4,
        GREATEROREQUAL = 5
    }

    public enum RELATIVEORABSOLUTE : byte
    {
        ABSOLUTE = 0,
        RELATIVE = 1
    }


    public enum SampleOrientation
    {
        Orientation1 = 0,
        Orientation2 = 90,
        Orientation3 = 270,
        Orientation4 = 360
    };

    public enum Units : byte
    {
        ADU = 0,

        count,

        [Description("LengthUnit")]
        mm,

        [Description("LengthUnit")]
        um,

        [Description("ResistivityUnit")]
        Ohmcm,

        [Description("TimeUnit")]
        sec

    };


    public enum ToolNames : byte
    {
        Unknown = 0,
        TTR,
        SHP,
        WSIChipping,
        WSIContamination,
        PED,
        MCI,
        PLI,
        UPCD
    }


    public enum CalculationTypes : byte
    {
        Unknown = 0,
        Average,
        StandardDeviation,
        Cpk,
        G_RAndR
    }

}
