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


        public SimpleCondition()
        {
        }


        public SimpleCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, Relativity relorabs, Relations validIf, double validIfValue)
            : base(name, calculationtype, value, relation, enabled, relorabs)
        {
            ValidIf = validIf;
            ValidIf_Value = validIfValue;
        }

        protected override IConditionEvaluationResult EvaluateCondition(IResult calculationResult, DateTime dateTime, IMeasurementSerie measSerie, IReferenceValue referenceValue)
        {
            if (!CheckCalculationType(calculationResult, CalculationType))
            {
                return false;
            }

            bool isMet = Compare((calculationResult as ISimpleCalculationResult).ResultValue);

            return new ConditionEvaluaitonResult(dateTime, measSerie, this, referenceValue, isMet, calculationResult);
        }

        //bool conditionEvaluationResult = condition.Compare(calcResult);
        //IConditionEvaluationResult conditionResult = new ConditionEvaluaitonResult(
        //    _parameters.DateTimeProvider.GetDateTime(),
        //    calcResult.Successful,
        //    calculationInputData,
        //    condition,
        //    referenceValue,
        //    conditionEvaluationResult,
        //    calcResult);


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
                    return $"{(RelOrAbs == Relativity.Absolute ? "(abs)" : "(rel)")} {LeftValue.ToString(format, formatProvider)} {ConditionRelation}";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
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

    }
}
