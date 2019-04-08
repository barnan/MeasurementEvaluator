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

        public ConditionBase(string name, CalculationTypes calculationtype, Relations relation, bool enabled, RELATIVEORABSOLUTE relorabs)
        {
            Name = name;
            CalculationType = calculationtype;
            ConditionRelation = relation;
            Enabled = enabled;
            RelOrAbs = relorabs;
        }

        #region INamed

        public string Name { get; set; }

        #endregion

        #region ICondition

        public CalculationTypes CalculationType { get; set; }
        public Relations ConditionRelation { get; set; }
        public bool Enabled { get; set; }
        public RELATIVEORABSOLUTE RelOrAbs { get; set; }


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
            return $"Name: {Name}{Environment.NewLine}Enabled: {Enabled}{Environment.NewLine}CalculationType: {CalculationType}{Environment.NewLine}RelativeOrAbsolute: {RelOrAbs}";
        }

        #endregion
    }



    public abstract class ConditionBase<T> : ConditionBase, IConditionHandler<T>
        where T : struct
    {
        public T Value { get; set; }


        public ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, bool enabled, RELATIVEORABSOLUTE relorabs)
            : base(name, calculationtype, relation, enabled, relorabs)
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

            switch (ConditionRelation.Value)
            {
                case Relations.RelationsEnumValues.LESS:
                    return compResult == -1;
                case Relations.RelationsEnumValues.GREATER:
                    return compResult == 1;
                case Relations.RelationsEnumValues.LESSOREQUAL:
                    return compResult == -1 && equality;
                case Relations.RelationsEnumValues.GREATEROREQUAL:
                    return compResult == 1 && equality;
                case Relations.RelationsEnumValues.EQUAL:
                    return equality;
                case Relations.RelationsEnumValues.NOTEQUAL:
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
