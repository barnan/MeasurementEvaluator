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

        protected override bool EvaluateCondition(IResult calculationResult)
        {
            if (!CheckCalculationType(calculationResult, CalculationType))
            {
                return false;
            }

            if (!(calculationResult is ISimpleCalculationResult simpleResult))
            {
                return false;
            }

            return Compare(simpleResult.ResultValue);
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}Valid, if {ValidIf} than {ValidIf_Value}";
        }

        #endregion


        public override XElement SaveToXml(XElement inputElement)
        {
            this.TrySave(Name, inputElement, nameof(Name));
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
