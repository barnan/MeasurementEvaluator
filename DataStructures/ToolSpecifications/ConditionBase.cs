using Interfaces;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{

    public abstract class ConditionBase : IConditionHandler
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

        #region INamed

        public string Name { get; set; }

        #endregion

        #region ICondition

        public CalculationTypes CalculationType { get; set; }
        public Relations ConditionRelation { get; set; }
        public bool Enabled { get; set; }

        public bool Compare(ICalculationResult calculationResult)
        {
            if (calculationResult == null)
            {
                return false;
            }
            return EvaluateCondition(calculationResult);
        }

        protected abstract bool EvaluateCondition(ICalculationResult calculationResult);

        #endregion

        #region object.ToString() override

        public override string ToString()
        {
            return $"Name: {Name}{Environment.NewLine}Enabled: {Enabled}{Environment.NewLine}CalculationType: {CalculationType}";
        }

        #endregion
    }



    public abstract class ConditionBase<T> : ConditionBase, IConditionHandler<T> where T : struct
    {
        public T Value { get; set; }


        public ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, bool enabled)
            : base(name, calculationtype, relation, enabled)
        {
            Value = value;
        }


        public ConditionBase()
        {
            Value = default(T);
        }


        // evaluation calls it from derived classes:
        protected bool Compare(T leftValue)
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


        protected bool CheckCalculationType(ICalculationResult calculationResult, CalculationTypes calculationType)
        {
            switch (calculationType)
            {
                case CalculationTypes.Unknown:
                    return false;
                case CalculationTypes.Average:
                    return calculationResult is ISimpleCalculationResult;
                case CalculationTypes.StandardDeviation:
                    return calculationResult is ISimpleCalculationResult;
                case CalculationTypes.Cp:
                    return calculationResult is IQCellsCalculationResult;
                case CalculationTypes.Cpk:
                    return calculationResult is IQCellsCalculationResult;
                case CalculationTypes.GRAndR:
                    return calculationResult is IGRAndRCalculationResult;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculationType), calculationType, null);
            }
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}True, if {ConditionRelation} than {Value}";
        }

        #endregion

    }
}
