using Interfaces.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Interfaces
{

    // type safe enum, but not sealed!!
    public class Relations
    {
        public string Name { get; }
        public int Value { get; }

        public Relations(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public class RelationsEnumValues
        {
            public const int EQUAL = 0;
            public const int NOTEQUAL = 1;
            public const int LESS = 2;
            public const int LESSOREQUAL = 3;
            public const int GREATER = 4;
            public const int GREATEROREQUAL = 5;
            public const int ALLWAYS = 6;
        }

        public static Relations EQUAL = new Relations(nameof(EQUAL), RelationsEnumValues.EQUAL);
        public static Relations NOTEQUAL = new Relations(nameof(NOTEQUAL), RelationsEnumValues.NOTEQUAL);
        public static Relations LESS = new Relations(nameof(LESS), RelationsEnumValues.LESS);
        public static Relations LESSOREQUAL = new Relations(nameof(LESSOREQUAL), RelationsEnumValues.LESSOREQUAL);
        public static Relations GREATER = new Relations(nameof(GREATER), RelationsEnumValues.GREATER);
        public static Relations GREATEROREQUAL = new Relations(nameof(GREATEROREQUAL), RelationsEnumValues.GREATEROREQUAL);
        public static Relations ALLWAYS = new Relations(nameof(ALLWAYS), RelationsEnumValues.ALLWAYS);

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator int(Relations rel)
        {
            return rel.Value;
        }


        public static explicit operator Relations(string val)
        {

            if (EQUAL.ToString() == val)
            {
                return EQUAL;
            }
            if (NOTEQUAL.ToString() == val)
            {
                return NOTEQUAL;
            }
            if (LESS.ToString() == val)
            {
                return LESS;
            }
            if (LESSOREQUAL.ToString() == val)
            {
                return LESSOREQUAL;
            }
            if (GREATER.ToString() == val)
            {
                return GREATER;
            }
            if (GREATEROREQUAL.ToString() == val)
            {
                return GREATEROREQUAL;
            }
            if (ALLWAYS.ToString() == val)
            {
                return ALLWAYS;
            }

            return null;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object other)
        {
            Relations otherRelation = other as Relations;
            if (ReferenceEquals(null, otherRelation))
            {
                return false;
            }
            return Value == otherRelation.Value;
        }
    }


    public enum RELATIVEORABSOLUTE : byte
    {
        ABSOLUTE = 0,
        RELATIVE = 1
    }


    public enum SampleOrientation
    {
        Orientation_0 = 0,
        Orientation_90 = 90,
        Orientation_180 = 270,
        Orientation_270 = 360
    };


    public enum Units : byte
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

    };


    // type safe enum pattern:
    public sealed class ToolNames
    {
        public string Name { get; }
        public int Value { get; }

        public ToolNames(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public static ToolNames Unknown = new ToolNames(nameof(Unknown), 0);
        public static ToolNames TTR = new ToolNames(nameof(TTR), 1);
        public static ToolNames SHP = new ToolNames(nameof(SHP), 2);
        public static ToolNames WSIChipping = new ToolNames(nameof(WSIChipping), 3);
        public static ToolNames WSIContamination = new ToolNames(nameof(WSIContamination), 4);
        public static ToolNames PED = new ToolNames(nameof(PED), 5);
        public static ToolNames MCI = new ToolNames(nameof(MCI), 6);
        public static ToolNames PLI = new ToolNames(nameof(PLI), 7);
        public static ToolNames UPCD = new ToolNames(nameof(UPCD), 8);

        public override string ToString()
        {
            return Name;
        }
    }


    public enum CalculationTypes : byte
    {
        Unknown = 0,
        Average,
        StandardDeviation,
        Cp,
        Cpk,
        GRAndR
    }


    public class ResultEventArgs : EventArgs
    {
        public IResult Result { get; }
        public ResultEventArgs(IResult result)
        {
            Result = result;
        }
    }


    public class DataCollectorResultEventArgs : EventArgs
    {
        List<string> SpecificationName { get; }
        List<string> MeasurementDataFileNames { get; }
        List<string> ReferenceName { get; }

        public DataCollectorResultEventArgs(List<string> specificationName, List<string> measurementDataFileNames, List<string> referenceName)
        {
            SpecificationName = specificationName;
            MeasurementDataFileNames = measurementDataFileNames;
            ReferenceName = referenceName;
        }
    }



    public class CustomEventArg<T> : EventArgs
    {
        private readonly T _data;

        public CustomEventArg(T data)
        {
            _data = data;
        }

        public T Data => _data;
    }


    public enum MessageSeverityLevels
    {
        Trace,
        Info,
        Warning,
        Error
    }

}
