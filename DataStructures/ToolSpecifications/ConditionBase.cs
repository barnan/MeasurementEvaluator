using Interfaces;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{

    public class ConditionBase<T> : ICondition<T>
    {
        public string Name { get; set; }

        public CalculationTypes CalculationType { get; set; }

        public Relations ConditionRelation { get; set; }

        public bool Enabled { get; set; }

        public T Value { get; set; }

        private readonly IComparer<T> _comparer;



        public ConditionBase(T value, Relations relation, bool enabled, IComparer<T> comparer)
        {
            Value = value;
            ConditionRelation = relation;
            Enabled = enabled;
            _comparer = comparer;
        }

        public ConditionBase()
        {
            Value = default(T);
            ConditionRelation = Relations.EQUAL;
            Enabled = false;
            _comparer = Comparer<T>.Default;
        }

        public override string ToString()
        {
            if (Enabled)
                return "Valid. It should be " + ConditionRelation.ToString() + " than " + Value.ToString();
            else
                return "NOT Valid. It should be " + ConditionRelation.ToString() + " than " + Value.ToString();
        }


        public bool Compare(T value1, Relations relation, T value2)
        {
            int calculatedrelation = _comparer.Compare(value1, value2);

            switch (calculatedrelation)
            {
                case -1:
                    if (relation == Relations.LESS || relation == Relations.LESSOREQUAL || relation == Relations.NOTEQUAL)
                    {
                        return true;
                    }
                    return false;
                case 1:
                    if (relation == Relations.GREATER || relation == Relations.GREATEROREQUAL || relation == Relations.NOTEQUAL)
                    {
                        return true;
                    }
                    return false;
                case 0:
                    if (relation == Relations.EQUAL || relation == Relations.GREATEROREQUAL)
                    {
                        return true;
                    }
                    return false;
            }
            return false;
        }


    }





}
