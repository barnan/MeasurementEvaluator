using Interfaces;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
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


        public SimpleCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, RELATIVEORABSOLUTE relorabs, Relations validIf, double validIfValue)
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
            inputElement.Add(new XElement(nameof(CalculationType), CalculationType));
            inputElement.Add(new XElement(nameof(ConditionRelation), ConditionRelation));
            inputElement.Add(new XElement(nameof(RelOrAbs), RelOrAbs));
            inputElement.Add(new XElement(nameof(Value), Value));
            inputElement.Add(new XElement(nameof(ValidIf), ValidIf));
            inputElement.Add(new XElement(nameof(ValidIf_Value), ValidIf_Value));

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
                    RelOrAbs = (RELATIVEORABSOLUTE)Enum.Parse(typeof(RELATIVEORABSOLUTE), xelement.Element(nameof(RelOrAbs)).Value);
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
