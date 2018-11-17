using Interfaces;
using Interfaces.ToolSpecifications;

namespace Measurement_Evaluator.BLL
{

    public class ConditionBase : ICondition
    {
        public string Name { get; set; }

        public CalculationTypes CalculationType { get; set; }

        public Relations ConditionRelation { get; set; }

        public bool Enabled { get; set; }

        public double Value { get; set; }


        public ConditionBase(double value, Relations relation, bool enabled)
        {
            Value = value;
            ConditionRelation = relation;
            Enabled = enabled;
        }

        public ConditionBase()
        {
        }

        public override string ToString()
        {
            if (Enabled)
                return "Valid. It should be " + ConditionRelation.ToString() + " than " + Value.ToString();
            else
                return "NOT Valid. It should be " + ConditionRelation.ToString() + " than " + Value.ToString();
        }


        public static bool Compare(double value1, Relations relation, double value2)
        {
            switch (relation)
            {
                case Relations.LESS:
                    return (value1 < value2);
                case Relations.GREATER:
                    return (value1 > value2);
                case Relations.LESSOREQUAL:
                    return (value1 <= value2);
                case Relations.GREATEROREQUAL:
                    return (value1 >= value2);
                case Relations.EQUAL:
                    return (value1 == value2);
                case Relations.NOTEQUAL:
                    return (value1 != value2);
            }
            return false;
        }

    }
}
