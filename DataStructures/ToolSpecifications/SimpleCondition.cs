using Interfaces;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;

namespace DataStructures.ToolSpecifications
{
    public class SimpleCondition : ConditionBase<double>, ISimpleConditionHandler
    {
        public Relations ValidIf { get; set; }

        public double ValidIf_Value { get; set; }


        public SimpleCondition()
            : base()
        {
            ValidIf = Relations.EQUAL;
            ValidIf_Value = 0;
        }


        public SimpleCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, Relations validIf, double validIf_Value)
            : base(name, calculationtype, value, relation, enabled)
        {
            ValidIf = validIf;
            ValidIf_Value = validIf_Value;
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


    }
}
