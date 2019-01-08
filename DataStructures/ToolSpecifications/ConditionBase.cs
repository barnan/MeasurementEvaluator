using Interfaces;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{

    public class ConditionBase : ICondition
    {
        public ConditionBase()
        {
        }

        public ConditionBase(string name, CalculationTypes calculationtype, Relations relation, bool enabled)
        {
            Name = name;
            CalculationType = calculationtype;
            ConditionRelation = relation;
            Enabled = enabled;
        }

        public string Name { get; }

        public CalculationTypes CalculationType { get; }

        public Relations ConditionRelation { get; }

        public bool Enabled { get; }


        public override string ToString()
        {
            return $"{(Enabled ? "Enabled" : "NOT Enabled")}.";
        }
    }



    public class ConditionBase<T> : ConditionBase, ICondition<T> where T : struct
    {
        public T Value { get; private set; }

        public ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, bool enabled)
            : base(name, calculationtype, relation, enabled)
        {
            Value = value;
        }


        public ConditionBase()
        {
            Value = default(T);
        }


        public virtual bool Compare(T leftValue)
        {
            bool equality = EqualityComparer<T>.Default.Equals(leftValue, Value);
            int compResult = Comparer<T>.Default.Compare(leftValue, Value);

            switch (ConditionRelation)
            {
                case Relations.LESS:
                    return compResult == -1;
                case Relations.GREATER:
                    return compResult == 1;
                case Relations.LESSOREQUAL:
                    return compResult == -1 && equality;
                case Relations.GREATEROREQUAL:
                    return compResult == 1 && equality;
                case Relations.EQUAL:
                    return equality;
                case Relations.NOTEQUAL:
                    return equality;
            }
            return false;
        }


        public override string ToString()
        {
            return $"{(Enabled ? "Enabled" : "NOT Enabled")}. It is true if " + ConditionRelation.ToString() + " than " + Value.ToString();
        }


    }
}
