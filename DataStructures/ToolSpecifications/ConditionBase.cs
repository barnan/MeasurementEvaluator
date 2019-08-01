using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{

    public abstract class ConditionBase : IConditionHandler
    {

        #region ctor

        public ConditionBase()
        {
        }

        public ConditionBase(string name, CalculationTypes calculationtype, Relations relation, bool enabled, Relativity relorabs)
        {
            Name = name;
            CalculationType = calculationtype;
            ConditionRelation = relation;
            Enabled = enabled;
        }

        #endregion

        #region INamed

        public string Name { get; set; }

        #endregion

        #region ICondition

        public CalculationTypes CalculationType { get; set; }
        public Relations ConditionRelation { get; set; }
        public bool Enabled { get; set; }


        public IConditionEvaluationResult Evaluate(IResult calculationResult, DateTime dateTime, IMeasurementSerie measSerie, IReferenceValue referenceValue)
        {
            if (calculationResult == null || measSerie == null)
            {
                return null;
            }

            return EvaluateCondition(calculationResult, dateTime, measSerie, referenceValue);
        }

        protected abstract IConditionEvaluationResult EvaluateCondition(IResult calculationResult, DateTime dateTimeProvider, IMeasurementSerie measSerie, IReferenceValue referenceValue);

        #endregion

        #region object.ToString() override

        public override string ToString()
        {
            return $"Name: {Name}{Environment.NewLine}Enabled: {Enabled}{Environment.NewLine}CalculationType: {CalculationType}";
        }

        public abstract string ToString(string format, IFormatProvider formatProvider);

        #endregion

        public abstract XElement SaveToXml(XElement inputElement);

        public abstract bool LoadFromXml(XElement inputElement);
    }





    public abstract class ConditionBase<T> : ConditionBase, IConditionHandler<T>
        where T : struct
    {

        public T LeftValue { get; set; }


        #region ctor

        protected ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, bool enabled, Relativity relorabs)
            : base(name, calculationtype, relation, enabled, relorabs)
        {
            LeftValue = value;
        }


        protected ConditionBase()
        {
            LeftValue = default(T);
        }

        #endregion

        // evaluation calls it from derived classes:
        protected bool Compare(T rightValue)
        {
            bool equality = EqualityComparer<T>.Default.Equals(LeftValue, rightValue);
            int compResult = Comparer<T>.Default.Compare(LeftValue, rightValue);

            switch (ConditionRelation.Relation)
            {
                case RelationsEnumValues.LESS:
                    return compResult == -1;
                case RelationsEnumValues.GREATER:
                    return compResult == 1;
                case RelationsEnumValues.LESSOREQUAL:
                    return compResult == 1 || equality;
                case RelationsEnumValues.GREATEROREQUAL:
                    return compResult == -1 || equality;
                case RelationsEnumValues.EQUAL:
                    return equality;
                case RelationsEnumValues.NOTEQUAL:
                    return equality;
            }
            return false;
        }

        //protected bool CheckCalculationType(IResult calculationResult, CalculationTypes calcType)
        //{
        //    if (CalculationType != calcType)
        //    {
        //        return false;
        //    }

        //    switch (calcType)
        //    {
        //        case CalculationTypes.Unknown:
        //            return false;
        //        case CalculationTypes.Average:
        //            return calculationResult is ISimpleCalculationResult;
        //        case CalculationTypes.StandardDeviation:
        //            return calculationResult is ISimpleCalculationResult;
        //        case CalculationTypes.Cp:
        //            return calculationResult is IQCellsCalculationResult;
        //        case CalculationTypes.Cpk:
        //            return calculationResult is IQCellsCalculationResult;
        //        case CalculationTypes.GRAndR:
        //            return calculationResult is IGRAndRCalculationResult;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(calcType), calcType, null);
        //    }
        //}

        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}{LeftValue}{ConditionRelation}";
        }

        #endregion

    }
}
