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

        public ConditionBase(string name, CalculationTypes calculationtype, Relations relation, Relativity relativity, bool enabled)
        {
            Name = name;
            CalculationType = calculationtype;
            Relativity = relativity;
            ConditionRelation = relation;
            Enabled = enabled;
        }

        #endregion

        #region INamed

        public string Name { get; set; }

        #endregion

        #region ICondition

        public CalculationTypes CalculationType { get; set; }
        public Relativity Relativity { get; set; }
        public Relations ConditionRelation { get; set; }
        public bool Enabled { get; set; }


        public IConditionEvaluationResult Evaluate(IResult calculationResult, DateTime dateTime, IReferenceValue referenceValue)
        {
            return EvaluateCondition(calculationResult, dateTime, referenceValue);
        }

        protected abstract IConditionEvaluationResult EvaluateCondition(IResult calculationResult, DateTime dateTimeProvider, IReferenceValue referenceValue);

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

        protected ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, Relativity relativity, bool enabled)
            : base(name, calculationtype, relation, relativity, enabled)
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
