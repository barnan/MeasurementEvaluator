using System.ComponentModel;

namespace Measurement_Evaluator.Interfaces.ReferenceSample
{

    public enum Dimensions : byte
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

    }


    public interface IReferenceValue
    {
        /// <summary>
        /// value name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Dimension of the Value
        /// </summary>
        Dimensions Dimension { get; }

        double Value { get; }

    }

    public interface IReferenceValueHandler : IReferenceValue
    {
        new string Name { get; set; }

        new Dimensions Dimension { get; set; }

        new double Value { get; set; }

    }


}
