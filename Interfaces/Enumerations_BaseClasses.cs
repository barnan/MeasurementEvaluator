using Interfaces.Result;
using System;
using System.Collections.Generic;
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



    public class EventArg<T> : EventArgs
    {
        // Property variable
        private readonly T p_EventData;

        // Constructor
        public EventArg(T data)
        {
            p_EventData = data;
        }

        // Property for EventArgs argument
        public T Data
        {
            get { return p_EventData; }
        }
    }



    public enum MessageLevels
    {
        Trace,
        Info,
        Warning,
        Error
    }

}
