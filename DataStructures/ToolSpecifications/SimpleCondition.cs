using Interfaces;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{
    public class SimpleCondition : ConditionBase<double>, ISimpleConditionHandler
    {

        private const string _XML_NODE = "Condition";


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

        protected override bool EvaluateCondition(ICalculationResult calculationResult)
        {
            if (!CheckCalculationType(calculationResult, CalculationType))
            {
                return false;
            }

            if (!(calculationResult is ISimpleCalculationResult simpleResult))
            {
                return false;
            }

            return Compare(simpleResult.Result);
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}Valid, if {ValidIf} than {ValidIf_Value}";
        }

        #endregion


        public override XElement SaveToXml(XElement inputElement)
        {
            inputElement.SetAttributeValue(nameof(Name), Name);
            this.TrySave(CalculationType, inputElement, nameof(CalculationType));
            this.TrySave(ConditionRelation, inputElement, nameof(ConditionRelation));
            this.TrySave(RelOrAbs, inputElement, nameof(RelOrAbs));
            this.TrySave(Value, inputElement, nameof(Value));
            this.TrySave(ValidIf, inputElement, nameof(ValidIf));
            this.TrySave(ValidIf_Value, inputElement, nameof(ValidIf_Value));

            return inputElement;
        }

        public override bool LoadFromXml(XElement inputElement)
        {
            foreach (XElement xelement in inputElement.Elements(nameof(SimpleCondition)))
            {
                var nameElement = xelement.Element(nameof(Name));

                if (Name == nameElement.Value)
                {
                    CalculationType = (CalculationTypes)Enum.Parse(typeof(CalculationTypes), xelement.Element(nameof(CalculationType)).Value);
                    ConditionRelation = (Relations)xelement.Element(nameof(ConditionRelation)).Value;
                    RelOrAbs = (Relativity)Enum.Parse(typeof(Relativity), xelement.Element(nameof(RelOrAbs)).Value);
                    Value = double.Parse(xelement.Element(nameof(Value)).Value);
                    ValidIf_Value = double.Parse(xelement.Element(nameof(ValidIf_Value)).Value);
                    ValidIf = (Relations)xelement.Element(nameof(ValidIf)).Value;

                    return true;
                }
            }

            return false;
        }

    }
}
