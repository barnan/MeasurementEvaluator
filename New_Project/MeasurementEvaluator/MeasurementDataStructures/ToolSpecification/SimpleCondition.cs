using System.Globalization;
using System.Xml.Linq;
using BaseClasses.MeasurementEvaluator;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace Evaluation.DataStructures.ToolSpecification
{
    public class SimpleCondition : ConditionBase, ISimpleConditionHandler
    {

        public Relations ValidIf { get; set; }

        public double ValidIf_Value { get; set; }


        #region ctor

        public SimpleCondition(string name, CalculationTypes calculationtype, Relations relation, bool enabled, Relations validIf, double validIfValue)
            : base(name, calculationtype, relation, enabled)
        {
            ValidIf = validIf;
            ValidIf_Value = validIfValue;
        }

        #endregion

        #region ICondition
        
        // evaluation calls it from derived classes:
        private bool CompareValidity(double rightValue)
        {
            bool equality = EqualityComparer<double>.Default.Equals(ValidIf_Value, rightValue);
            int compResult = Comparer<double>.Default.Compare(ValidIf_Value, rightValue);

            return ValidIf.Evaluation(equality, compResult);
        }

        #endregion

        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}Valid, if {ValidIf} than {ValidIf_Value}";
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            switch (format.ToUpperInvariant())
            {
                case null:
                case "":
                case "G":
                    return ToString();
                case "GRID":
                    return $"{LeftValue.ToString("N2")} {Relation}";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }

        #endregion


        #region XmlStorable

        public override XElement SaveToXml(XElement inputElement)
        {
            throw new NotImplementedException();

            //this.TrySave(Name, inputElement, nameof(Name));
            //this.TrySave(Enabled, inputElement, nameof(Enabled));
            //this.TrySave(CalculationTypeHandler, inputElement, nameof(CalculationTypeHandler));
            //this.TrySave(ConditionRelation, inputElement, nameof(ConditionRelation));
            //this.TrySave(LeftValue, inputElement, nameof(LeftValue));
            //this.TrySave(ValidIf, inputElement, nameof(ValidIf));
            //this.TrySave(ValidIf_Value, inputElement, nameof(ValidIf_Value));

            return inputElement;
        }

        public override bool LoadFromXml(XElement inputElement)
        {
            throw new NotImplementedException();

            //this.TryLoad(inputElement, nameof(Name));
            //this.TryLoad(inputElement, nameof(Enabled));
            //this.TryLoad(inputElement, nameof(CalculationTypeHandler));
            //this.TryLoad(inputElement, nameof(ConditionRelation));
            //this.TryLoad(inputElement, nameof(LeftValue));
            //this.TryLoad(inputElement, nameof(ValidIf));
            //this.TryLoad(inputElement, nameof(ValidIf_Value));
            return true;
        }

        #endregion

    }
}
