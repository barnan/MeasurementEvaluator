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


    // type safe enum pattern:
    public sealed class ToolNames
        {
        private readonly string _name;
        private readonly int _value;

        public ToolNames(string name, int value)
            {
            _name = name;
            _value = value;
            }


        public static ToolNames Unknown = new ToolNames(nameof(Unknown), 0);
        public static ToolNames TTR = new ToolNames(nameof(TTR), 0);
        public static ToolNames SHP = new ToolNames(nameof(SHP), 0);
        public static ToolNames WSIChipping = new ToolNames(nameof(WSIChipping), 0);
        public static ToolNames WSIContamination = new ToolNames(nameof(WSIContamination), 0);
        public static ToolNames PED = new ToolNames(nameof(PED), 0);
        public static ToolNames MCI = new ToolNames(nameof(MCI), 0);
        public static ToolNames PLI = new ToolNames(nameof(PLI), 0);
        public static ToolNames UPCD = new ToolNames(nameof(UPCD), 0);

        public override string ToString()
            {
            return _name;
            }

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
