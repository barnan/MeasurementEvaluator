using Interfaces;
using Interfaces.ToolSpecifications;

namespace DataStructures.ToolSpecifications
{
    public class SimpleCondition : ConditionBase<double>, ISimpleCondition
    {
        public Relations ValidIf { get; }

        public double ValidIf_Value { get; }



        public SimpleCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, Relations validIf, double validIf_Value)
            : base(name, calculationtype, value, relation, enabled)
        {
            ValidIf = validIf;
            ValidIf_Value = validIf_Value;
        }


    }
}
