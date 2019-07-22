using Interfaces.Misc;
using Interfaces.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace Interfaces
{

    /// <summary>
    /// 
    /// </summary>
    public sealed class Relations : IXmlStorable
    {
        private const string XELEMENT_ATTRIBUTE_NAME = "Value";

        public string Name { get; private set; }
        public int Value { get; private set; }

        public Relations(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public Relations()
        {
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

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object other)
        {
            Relations otherRelation = other as Relations;
            if (ReferenceEquals(null, otherRelation))
            {
                return false;
            }
            return Value == otherRelation.Value;
        }

        public XElement SaveToXml(XElement inputElement)
        {
            inputElement.SetAttributeValue(XELEMENT_ATTRIBUTE_NAME, Name);
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            string attributeValue = inputElement.Attribute(XELEMENT_ATTRIBUTE_NAME).Value;
            var element = (Relations)attributeValue;
            Name = element.Name;
            Value = element.Value;
            return true;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public enum Relativity
    {
        Absolute = 0,
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
    };



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

    };



    /// <summary>
    /// 
    /// </summary>
    public sealed class ToolNames : IXmlStorable
    {
        private const string XELEMENT_TOOLNAME = "ToolName";

        public string Name { get; private set; }
        public int Value { get; private set; }

        public ToolNames(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public ToolNames()
        {
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

        public XElement SaveToXml(XElement inputElement)
        {
            inputElement.SetAttributeValue(XELEMENT_TOOLNAME, Name);
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            string attributeValue = inputElement.Attribute(XELEMENT_TOOLNAME).Value;
            var element = (ToolNames)attributeValue;
            Name = element.Name;
            Value = element.Value;
            return true;
        }

        public static explicit operator ToolNames(string val)
        {
            if (val == Unknown.Name)
            {
                return Unknown;
            }
            if (val == TTR.Name)
            {
                return TTR;
            }
            if (val == SHP.Name)
            {
                return SHP;
            }
            if (val == WSIChipping.Name)
            {
                return WSIChipping;
            }
            if (val == WSIContamination.Name)
            {
                return WSIContamination;
            }
            if (val == PED.Name)
            {
                return PED;
            }
            if (val == MCI.Name)
            {
                return MCI;
            }
            if (val == PLI.Name)
            {
                return PLI;
            }
            if (val == UPCD.Name)
            {
                return UPCD;
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is ToolNames toolNameParam))
            {
                return false;
            }

            return Value == toolNameParam.Value && Name == toolNameParam.Name;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() + Name.GetHashCode();
        }

        public static bool operator ==(ToolNames toolName1, ToolNames toolName2)
        {
            if (object.Equals(toolName1, null) && object.Equals(toolName2, null))
                return true;

            return !object.Equals(toolName1, null) && !object.Equals(toolName2, null) && toolName1.Value == toolName2.Value && toolName1.Name == toolName2.Name;
        }

        public static bool operator !=(ToolNames toolName1, ToolNames ToolName2)
        {
            return !(toolName1 == ToolName2);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public enum CalculationTypes
    {
        Unknown = 0,
        Average,
        StandardDeviation,
        Cp,
        Cpk,
        GRAndR
    }


    /// <summary>
    /// 
    /// </summary>
    public class ResultEventArgs : EventArgs
    {
        public IResult Result { get; }
        public ResultEventArgs(IResult result)
        {
            Result = result;
        }
    }



    /// <summary>
    /// 
    /// </summary>
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



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomEventArg<T> : EventArgs
    {
        private readonly T _data;

        public CustomEventArg(T data)
        {
            _data = data;
        }

        public T Data => _data;
    }

}
