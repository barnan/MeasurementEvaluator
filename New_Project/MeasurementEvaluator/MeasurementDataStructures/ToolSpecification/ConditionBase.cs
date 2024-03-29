﻿using System.Xml.Linq;
using Interfaces;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification
{
    public abstract class ConditionBase : ICondition
    {
        protected ConditionBase(string name, CalculationTypes calculationType, Relations relation, bool enabled)
        {
            Name = name;
            CalculationType = calculationType;
            Relation = relation;
            IsEnabled = enabled;
        }

        public virtual XElement SaveToXml(XElement inputElement)
        {
            throw new NotImplementedException();
        }

        public virtual bool LoadFromXml(XElement inputElement)
        {
            throw new NotImplementedException();
        }

        public virtual string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"Name: {Name}{Environment.NewLine}Enabled: {IsEnabled}{Environment.NewLine}CalculationType: {CalculationType}";
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        private CalculationTypes _calculationType;
        public CalculationTypes CalculationType
        {
            get { return _calculationType; }
            set { _calculationType = value; }
        }

        private Relativities _relativity;
        public Relativities Relativity
        {
            get { return _relativity; }
            set { _relativity = value; }
        }

        private Relations _relation;
        public Relations Relation
        {
            get { return _relation; }
            set { _relation = value; }
        }
    }


    //public abstract class ConditionBase<T> : ConditionBase, IConditionHandler<T>
    //    where T : struct
    //{

    //    public T LeftValue { get; set; }


    //    #region ctor

    //    protected ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, bool enabled)
    //        : base(name, calculationtype, relation, enabled)
    //    {
    //        LeftValue = value;
    //    }


    //    protected ConditionBase() 
    //    {
    //        LeftValue = default(T);
    //    }

    //    #endregion

    //    #region object.ToString()

    //    public override string ToString()
    //    {
    //        return $"{base.ToString()}{Environment.NewLine}{LeftValue}{Relation}";
    //    }

    //    #endregion

    //}
}
