using Interfaces;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{

    public class ConditionBase : ICondition
    {
        public string Name { get; }
        public CalculationTypes CalculationType { get; }
        public Relations ConditionRelation { get; }
        public bool Enabled { get; }


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

        #region IFormattable

        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return $"Name: {Name}{Environment.NewLine}Enabled: {Enabled}{Environment.NewLine}CalculationType: {CalculationType}";
        }

        #endregion
    }



    public class ConditionBase<T> : ConditionBase, ICondition<T> where T : struct
    {
        public T Value { get; }


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


        #region IFormattable

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{base.ToString()}{Environment.NewLine}True, if {ConditionRelation} than {Value}";
        }

        #endregion

    }
}
