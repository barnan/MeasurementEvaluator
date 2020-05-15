using Interfaces.Misc;
using System;
using System.Xml.Linq;

namespace Interfaces.BaseClasses
{

    public sealed class CalculationTypeHandler : IXmlStorable, IEquatable<CalculationTypeHandler>
    {

        private const string XELEMENT_CALCULATIONTYPE = "CalculationType";
        private const string XELEMENT_RELATIVITY = "Relativity";


        public Relativity[] AvailableRelativities { get; private set; }


        public Relativity Relativity { get; set; }


        public CalculationType CalculationType { get; private set; }


        public CalculationTypeHandler()
        {
        }

        private CalculationTypeHandler(CalculationType value, Relativity[] relativities)
        {
            AvailableRelativities = relativities;
            CalculationType = value;
        }


        public static CalculationTypeHandler Unknown => new CalculationTypeHandler(CalculationType.Unknown, Array.Empty<Relativity>());
        public static CalculationTypeHandler Average => new CalculationTypeHandler(CalculationType.Average, new Relativity[] { Relativity.Absolute, Relativity.Relative });
        public static CalculationTypeHandler StandardDeviation => new CalculationTypeHandler(CalculationType.StandardDeviation, new Relativity[] { Relativity.Absolute, Relativity.Relative });
        public static CalculationTypeHandler Cp => new CalculationTypeHandler(CalculationType.Cp, new Relativity[] { Relativity.Absolute });
        public static CalculationTypeHandler Cpk => new CalculationTypeHandler(CalculationType.Cpk, new Relativity[] { Relativity.Absolute });
        public static CalculationTypeHandler GRAndR => new CalculationTypeHandler(CalculationType.GRAndR, new Relativity[] { Relativity.Absolute });


        #region XmlStorable

        public XElement SaveToXml(XElement inputElement)
        {
            inputElement.SetAttributeValue(XELEMENT_CALCULATIONTYPE, CalculationType);
            inputElement.SetAttributeValue(XELEMENT_RELATIVITY, Relativity);
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            string calculationTypeString = inputElement.Attribute(XELEMENT_CALCULATIONTYPE)?.Value;
            string relativityValue = inputElement.Attribute(XELEMENT_RELATIVITY)?.Value;

            switch (relativityValue)
            {
                case nameof(Relativity.Relative):
                    Relativity = Relativity.Relative;
                    break;
                case nameof(Relativity.Absolute):
                    Relativity = Relativity.Relative;
                    break;
                default:
                    return false;
            }

            switch (calculationTypeString)
            {
                case nameof(Average):
                    CalculationType = CalculationType.Average;
                    AvailableRelativities = Average.AvailableRelativities;
                    break;
                case nameof(StandardDeviation):
                    CalculationType = CalculationType.StandardDeviation;
                    AvailableRelativities = StandardDeviation.AvailableRelativities;
                    break;
                case nameof(Cp):
                    CalculationType = CalculationType.Cp;
                    AvailableRelativities = Cp.AvailableRelativities;
                    break;
                case nameof(Cpk):
                    CalculationType = CalculationType.Cpk;
                    AvailableRelativities = Cpk.AvailableRelativities;
                    break;
                case nameof(GRAndR):
                    CalculationType = CalculationType.GRAndR;
                    AvailableRelativities = GRAndR.AvailableRelativities;
                    break;
                case nameof(Unknown):
                default:
                    return false;
            }

            return true;
        }

        #endregion

        #region IEquatable

        public bool Equals(CalculationTypeHandler other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj is CalculationTypeHandler types && this == types;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (AvailableRelativities != null ? AvailableRelativities.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Relativity;
                hashCode = (hashCode * 397) ^ (int)CalculationType;
                return hashCode;
            }
        }

        #endregion

        #region == operator overloading

        public static bool operator ==(CalculationTypeHandler left, CalculationTypeHandler right)
        {
            if (ReferenceEquals(null, right))
            {
                return false;
            }
            if (ReferenceEquals(null, left))
            {
                return true;
            }
            return left.CalculationType == right.CalculationType;
        }

        public static bool operator !=(CalculationTypeHandler left, CalculationTypeHandler right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region object.ToString()

        public override string ToString()
        {
            return $"({Relativity}) {CalculationType}";
        }

        #endregion
    }

}
