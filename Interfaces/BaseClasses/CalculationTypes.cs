using Interfaces.Misc;
using System;
using System.Xml.Linq;

namespace Interfaces.BaseClasses
{

    public enum CalculationTypesValues
    {
        Unknown = 0,
        Average = 1,
        StandardDeviation = 2,
        Cp = 3,
        Cpk = 4,
        GRAndR = 5,
    }


    public sealed class CalculationTypes : IXmlStorable, IEquatable<CalculationTypes>

    {
        private const string XELEMENT_CALCULATIONTYPE = "CalculationType";
        private const string XELEMENT_RELATIVITY = "Relativity";

        public Relativity[] Relativites { get; private set; }

        public Relativity Relativity { get; set; }


        public CalculationTypesValues CalculationTypeValue { get; private set; }


        public CalculationTypes()
        {
        }

        private CalculationTypes(CalculationTypesValues value, Relativity[] relativites)
        {
            Relativites = relativites;
            CalculationTypeValue = value;
        }


        public static readonly CalculationTypes Unknown = new CalculationTypes(CalculationTypesValues.Unknown, Array.Empty<Relativity>());
        public static readonly CalculationTypes Average = new CalculationTypes(CalculationTypesValues.Average, new Relativity[] { Relativity.Absolute });
        public static readonly CalculationTypes StandardDeviation = new CalculationTypes(CalculationTypesValues.StandardDeviation, new Relativity[] { Relativity.Absolute, Relativity.Relative });
        public static readonly CalculationTypes Cp = new CalculationTypes(CalculationTypesValues.Cp, new Relativity[] { Relativity.Absolute });
        public static readonly CalculationTypes Cpk = new CalculationTypes(CalculationTypesValues.Cpk, new Relativity[] { Relativity.Absolute });
        public static readonly CalculationTypes GRAndR = new CalculationTypes(CalculationTypesValues.GRAndR, new Relativity[] { Relativity.Absolute });


        public static explicit operator CalculationTypes(CalculationTypesValues typeValues)
        {
            switch (typeValues)
            {
                case CalculationTypesValues.Average:
                    return Average;
                case CalculationTypesValues.StandardDeviation:
                    return StandardDeviation;
                case CalculationTypesValues.Cp:
                    return Cp;
                case CalculationTypesValues.Cpk:
                    return Cpk;
                case CalculationTypesValues.GRAndR:
                    return GRAndR;
                case CalculationTypesValues.Unknown:
                default:
                    return Unknown;
            }
        }

        public XElement SaveToXml(XElement inputElement)
        {
            inputElement.SetAttributeValue(XELEMENT_CALCULATIONTYPE, CalculationTypeValue);
            inputElement.SetAttributeValue(XELEMENT_RELATIVITY, Relativity);
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            var calculationTypeAttrib = inputElement.Attribute(XELEMENT_CALCULATIONTYPE);
            var relativityAttrib = inputElement.Attribute(XELEMENT_RELATIVITY);
            if (calculationTypeAttrib == null || relativityAttrib == null)
            {
                return false;
            }

            string calculationTypeValue = calculationTypeAttrib.Value;
            string relativityValue = relativityAttrib.Value;

            switch (relativityValue)
            {
                case nameof(Relativity.Relative):
                    Relativity = Relativity.Relative;
                    break;
                case nameof(Relativity.Absolute):
                default:
                    Relativity = Relativity.Absolute;
                    break;
            }

            switch (calculationTypeValue)
            {
                case nameof(Average):
                    CalculationTypeValue = CalculationTypesValues.Average;
                    Relativites = Average.Relativites;
                    break;
                case nameof(StandardDeviation):
                    CalculationTypeValue = CalculationTypesValues.StandardDeviation;
                    Relativites = StandardDeviation.Relativites;
                    break;
                case nameof(Cp):
                    CalculationTypeValue = CalculationTypesValues.Cp;
                    Relativites = Cp.Relativites;
                    break;
                case nameof(Cpk):
                    CalculationTypeValue = CalculationTypesValues.Cpk;
                    Relativites = Cpk.Relativites;
                    break;
                case nameof(GRAndR):
                    CalculationTypeValue = CalculationTypesValues.GRAndR;
                    Relativites = GRAndR.Relativites;
                    break;
                case nameof(Unknown):
                default:
                    CalculationTypeValue = CalculationTypesValues.Unknown;
                    Relativites = Unknown.Relativites;
                    break;
            }

            return true;
        }

        public bool Equals(CalculationTypes other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return CalculationTypeValue == other.CalculationTypeValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj is CalculationTypes && Equals((CalculationTypes)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Relativites != null ? Relativites.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Relativity;
                hashCode = (hashCode * 397) ^ (int)CalculationTypeValue;
                return hashCode;
            }
        }

        public static bool operator ==(CalculationTypes left, CalculationTypes right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CalculationTypes left, CalculationTypes right)
        {
            return !left.Equals(right);
        }

    }

}
