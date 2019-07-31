using System.ComponentModel;

namespace Interfaces.BaseClasses
{
    /// <summary>
    /// 
    /// </summary>
    public enum Relativity
    {
        [Description("ABSOLUTE")]
        Absolute = 0,

        [Description("RELATIVE")]
        Relative = 1
    }


    /// <summary>
    /// 
    /// </summary>
    public enum SampleOrientation
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



    /// <summary>
    /// 
    /// </summary>
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



}
