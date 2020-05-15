using Interfaces.BaseClasses;
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

        public ConditionBase(string name, CalculationTypeHandler calculationtype, RelationHandler relation, bool enabled)
        {
            Name = name;
            CalculationTypeHandler = calculationtype;
            ConditionRelation = relation;
            Enabled = enabled;
        }

        #endregion

        #region INamed

        public string Name { get; set; }

        #endregion

        #region ICondition

        public CalculationTypeHandler CalculationTypeHandler { get; set; }
        public RelationHandler ConditionRelation { get; set; }
        public bool Enabled { get; set; }


        public IConditionEvaluationResult EvaluateCondition(IResult calculationResult, DateTime dateTime, IReferenceValue referenceValue)
        {
            return InternalEvaluate(calculationResult, dateTime, referenceValue);
        }

        protected abstract IConditionEvaluationResult InternalEvaluate(IResult calculationResult, DateTime dateTimeProvider, IReferenceValue referenceValue);

        #endregion

        #region object.ToString() override

        public override string ToString()
        {
            return $"Name: {Name}{Environment.NewLine}Enabled: {Enabled}{Environment.NewLine}CalculationType: {CalculationTypeHandler}";
        }

        public abstract string ToString(string format, IFormatProvider formatProvider);

        #endregion

        #region IXmlStorable

        public abstract XElement SaveToXml(XElement inputElement);

        public abstract bool LoadFromXml(XElement inputElement);

        #endregion
    }




    public abstract class ConditionBase<T> : ConditionBase, IConditionHandler<T>
        where T : struct
    {

        public T LeftValue { get; set; }


        #region ctor

        protected ConditionBase(string name, CalculationTypeHandler calculationtype, T value, RelationHandler relation, bool enabled)
            : base(name, calculationtype, relation, enabled)
        {
            LeftValue = value;
        }


        protected ConditionBase()
        {
            LeftValue = default(T);
        }

        #endregion

        protected bool EvaluateRelation(T rightValue)
        {
            bool equality = EqualityComparer<T>.Default.Equals(LeftValue, rightValue);
            int compResult = Comparer<T>.Default.Compare(LeftValue, rightValue);

            return ConditionRelation.Evaluation(equality, compResult);
        }

        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}{LeftValue}{ConditionRelation}";
        }

        #endregion

    }
}
