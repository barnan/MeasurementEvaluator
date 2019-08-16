using System.ComponentModel;

namespace Interfaces.BaseClasses
{

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



    //public sealed class CalculationTypes : IXmlStorable, IEquatable<CalculationTypes>
    //{

    //    private const string XELEMENT_CALCULATIONTYPE = "CalculationType";
    //    private const string XELEMENT_RELATIVITY = "Relativity";


    //    public Relativity[] AvailableRelativities { get; private set; }


    //    public Relativity Relativity { get; set; }


    //    public CalculationTypesValues CalculationTypeValue { get; private set; }


    //    public CalculationTypes()
    //    {
    //    }

    //    private CalculationTypes(CalculationTypesValues value, Relativity[] relativities)
    //    {
    //        AvailableRelativities = relativities;
    //        CalculationTypeValue = value;
    //    }


    //    public static CalculationTypes Unknown => new CalculationTypes(CalculationTypesValues.Unknown, Array.Empty<Relativity>());
    //    public static CalculationTypes Average => new CalculationTypes(CalculationTypesValues.Average, new Relativity[] { Relativity.Absolute });
    //    public static CalculationTypes StandardDeviation => new CalculationTypes(CalculationTypesValues.StandardDeviation, new Relativity[] { Relativity.Absolute, Relativity.Relative });
    //    public static CalculationTypes Cp => new CalculationTypes(CalculationTypesValues.Cp, new Relativity[] { Relativity.Absolute });
    //    public static CalculationTypes Cpk => new CalculationTypes(CalculationTypesValues.Cpk, new Relativity[] { Relativity.Absolute });
    //    public static CalculationTypes GRAndR => new CalculationTypes(CalculationTypesValues.GRAndR, new Relativity[] { Relativity.Absolute });


    //    #region XmlStorable

    //    public XElement SaveToXml(XElement inputElement)
    //    {
    //        inputElement.SetAttributeValue(XELEMENT_CALCULATIONTYPE, CalculationTypeValue);
    //        inputElement.SetAttributeValue(XELEMENT_RELATIVITY, Relativity);
    //        return inputElement;
    //    }

    //    public bool LoadFromXml(XElement inputElement)
    //    {
    //        string calculationTypeValue = inputElement.Attribute(XELEMENT_CALCULATIONTYPE)?.Value;
    //        string relativityValue = inputElement.Attribute(XELEMENT_RELATIVITY)?.Value;

    //        switch (relativityValue)
    //        {
    //            case nameof(Relativity.Relative):
    //                Relativity = Relativity.Relative;
    //                break;
    //            case nameof(Relativity.Absolute):
    //                Relativity = Relativity.Relative;
    //                break;
    //            default:
    //                return false;
    //        }

    //        switch (calculationTypeValue)
    //        {
    //            case nameof(Average):
    //                CalculationTypeValue = CalculationTypesValues.Average;
    //                AvailableRelativities = Average.AvailableRelativities;
    //                break;
    //            case nameof(StandardDeviation):
    //                CalculationTypeValue = CalculationTypesValues.StandardDeviation;
    //                AvailableRelativities = StandardDeviation.AvailableRelativities;
    //                break;
    //            case nameof(Cp):
    //                CalculationTypeValue = CalculationTypesValues.Cp;
    //                AvailableRelativities = Cp.AvailableRelativities;
    //                break;
    //            case nameof(Cpk):
    //                CalculationTypeValue = CalculationTypesValues.Cpk;
    //                AvailableRelativities = Cpk.AvailableRelativities;
    //                break;
    //            case nameof(GRAndR):
    //                CalculationTypeValue = CalculationTypesValues.GRAndR;
    //                AvailableRelativities = GRAndR.AvailableRelativities;
    //                break;
    //            case nameof(Unknown):
    //            default:
    //                return false;
    //        }

    //        return true;
    //    }

    //    #endregion

    //    #region IEquatable

    //    public bool Equals(CalculationTypes other)
    //    {
    //        return this == other;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (ReferenceEquals(this, obj))
    //        {
    //            return true;
    //        }
    //        return obj is CalculationTypes types && this == types;
    //    }

    //    public override int GetHashCode()
    //    {
    //        unchecked
    //        {
    //            var hashCode = (AvailableRelativities != null ? AvailableRelativities.GetHashCode() : 0);
    //            hashCode = (hashCode * 397) ^ (int)Relativity;
    //            hashCode = (hashCode * 397) ^ (int)CalculationTypeValue;
    //            return hashCode;
    //        }
    //    }

    //    #endregion

    //    #region == operator overloading

    //    public static bool operator ==(CalculationTypes left, CalculationTypes right)
    //    {
    //        if (ReferenceEquals(null, right))
    //        {
    //            return false;
    //        }
    //        if (ReferenceEquals(null, left))
    //        {
    //            return true;
    //        }
    //        return left.CalculationTypeValue == right.CalculationTypeValue;
    //    }

    //    public static bool operator !=(CalculationTypes left, CalculationTypes right)
    //    {
    //        return !left.Equals(right);
    //    }

    //    #endregion

    //    #region object.ToString()

    //    public override string ToString()
    //    {
    //        return $"({Relativity}) {CalculationTypeValue}";
    //    }

    //    #endregion
    //}

}
