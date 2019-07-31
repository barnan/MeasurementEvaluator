using DataStructures.ToolSpecifications.Results;
using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{
    public class SimpleCondition : ConditionBase<double>, ISimpleConditionHandler
    {

        public Relations ValidIf { get; set; }

        public double ValidIf_Value { get; set; }


        #region ctor

        public SimpleCondition()
        {
        }


        public SimpleCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, Relativity relorabs, Relations validIf, double validIfValue)
            : base(name, calculationtype, value, relation, enabled, relorabs)
        {
            ValidIf = validIf;
            ValidIf_Value = validIfValue;
        }

        #endregion

        #region ICondition

        protected override IConditionEvaluationResult EvaluateCondition(IResult result, DateTime dateTime, IMeasurementSerie measSerie, IReferenceValue referenceValue)
        {
            //if (!CheckCalculationType(result, CalculationType))
            //{
            //    return null;
            //}

            ISimpleCalculationResult calculationResult = result as ISimpleCalculationResult;

            if (!CompareValidity(calculationResult.Average))
            {
                return null;
                // log
            }

            bool isMet = false;
            //switch (RelOrAbs)
            //{
            //    case Relativity.Absolute:
            //        isMet = Compare(calculationResult.ResultValue);
            //        break;
            //    case Relativity.Relative:
            //        isMet = Compare(calculationResult.ResultValue / calculationResult.Average * 100);
            //        break;
            //}
            return new ConditionEvaluationResult(dateTime, this, referenceValue, isMet, calculationResult);
        }



        // evaluation calls it from derived classes:
        private bool CompareValidity(double rightValue)
        {
            switch (ValidIf.Relation)
            {
                case RelationsEnumValues.ALLWAYS:
                    return true;

                case RelationsEnumValues.LESS:
                    return ValidIf_Value < rightValue;

                case RelationsEnumValues.GREATER:
                    return ValidIf_Value > rightValue;

                case RelationsEnumValues.LESSOREQUAL:
                    return ValidIf_Value <= rightValue;

                case RelationsEnumValues.GREATEROREQUAL:
                    return ValidIf_Value >= rightValue;

                case RelationsEnumValues.EQUAL:
                    return ValidIf_Value == rightValue;

                case RelationsEnumValues.NOTEQUAL:
                    return ValidIf_Value != rightValue;
            }
            return false;
        }

        #endregion

        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}Valid, if {ValidIf} than {ValidIf_Value}";
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
                    return $"{(RelOrAbs == Relativity.Absolute ? "(abs)" : "(rel)")} {LeftValue.ToString("N2")} {ConditionRelation}";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }

        #endregion


        #region XmlStorable

        public override XElement SaveToXml(XElement inputElement)
        {
            this.TrySave(Name, inputElement, nameof(Name));
            this.TrySave(Enabled, inputElement, nameof(Enabled));
            this.TrySave(CalculationType, inputElement, nameof(CalculationType));
            this.TrySave(ConditionRelation, inputElement, nameof(ConditionRelation));
            this.TrySave(RelOrAbs, inputElement, nameof(RelOrAbs));
            this.TrySave(LeftValue, inputElement, nameof(LeftValue));
            this.TrySave(ValidIf, inputElement, nameof(ValidIf));
            this.TrySave(ValidIf_Value, inputElement, nameof(ValidIf_Value));

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
            this.TryLoad(inputElement, nameof(ValidIf));
            this.TryLoad(inputElement, nameof(ValidIf_Value));
            return true;
        }

        #endregion

    }
}
