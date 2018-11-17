using System.ComponentModel;

namespace Interfaces
{
    public enum Relations : byte
    {
        EQUAL = 0,
        NOTEQUAL,
        LESS,
        LESSOREQUAL,
        GREATER,
        GREATEROREQUAL
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


    public enum CalculationTypes
    {
        Unknown = 0,
        Average,
        StandardDeviation,
        Cpk
    }

}
