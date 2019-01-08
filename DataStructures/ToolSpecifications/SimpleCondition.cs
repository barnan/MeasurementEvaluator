using Interfaces;
using Interfaces.ToolSpecifications;
using System;
using System.Text;

namespace DataStructures.ToolSpecifications
{
    public class SimpleCondition : ConditionBase<double>, ISimpleCondition
    {
        public Relations ValidIf { get; }

        public double ValidIf_Value { get; }


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


        #region IFormattable

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder(base.ToString());

            sb.AppendLine($"Valid, if {ValidIf} than {ValidIf_Value}");

            return sb.ToString();
        }

        #endregion


    }
}
