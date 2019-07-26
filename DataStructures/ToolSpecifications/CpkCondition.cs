using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{
    public class CpkCondition : ConditionBase<double>, ICpkConditionHandler
    {
        public double HalfTolerance { get; set; }


        public CpkCondition()
            : base()
        {
            HalfTolerance = 0;
        }


        public CpkCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, Relativity relativity, double halfTolerance)
            : base(name, calculationtype, value, relation, enabled, relativity)
        {
            HalfTolerance = halfTolerance;
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}HalfTolerance: {HalfTolerance}{Environment.NewLine}{RelOrAbs}";
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            switch (format.ToUpperInvariant())
            {
                case "G":
                    return ToString();
                case "GRID":
                    return $"{LeftValue.ToString(format, formatProvider)} {ConditionRelation}";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }

        #endregion

        #region protected

        protected override bool EvaluateCondition(IResult calculationResult, IDateTimeProvider dateTimeProvider, IMeasurementSerie measSerie, IReferenceValue referenceValue)
        {
            if (!CheckCalculationType(calculationResult, CalculationType))
            {
                return false;
            }

            return Compare((calculationResult as IQCellsCalculationResult).ResultValue);
        }

        #endregion


        public override XElement SaveToXml(XElement inputElement)
        {
            this.TrySave(Name, inputElement, nameof(Name));
            this.TrySave(Enabled, inputElement, nameof(Enabled));
            this.TrySave(CalculationType, inputElement, nameof(CalculationType));
            this.TrySave(ConditionRelation, inputElement, nameof(ConditionRelation));
            this.TrySave(RelOrAbs, inputElement, nameof(RelOrAbs));
            this.TrySave(LeftValue, inputElement, nameof(LeftValue));
            this.TrySave(HalfTolerance, inputElement, nameof(HalfTolerance));

            return inputElement;
        }

        public override bool LoadFromXml(XElement inputElement)
        {
            this.TryLoad(inputElement, nameof(Name));
            this.TryLoad(inputElement, nameof(Enabled));
            this.TryLoad(inputElement, nameof(CalculationType));
            this.TryLoad(inputElement, nameof(ConditionRelation));
            this.TryLoad(inputElement, nameof(RelOrAbs));
            this.TryLoad(inputElement, nameof(LeftValue));
            this.TryLoad(inputElement, nameof(HalfTolerance));
            return true;
        }
    }

}
