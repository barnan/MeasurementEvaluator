﻿using Interfaces.BaseClasses;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{

    public abstract class ConditionBase : IConditionHandler
    {

        public ConditionBase()
        {
        }

        public ConditionBase(string name, CalculationTypes calculationtype, Relations relation, bool enabled, Relativity relorabs)
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
        public Relativity RelOrAbs { get; set; }


        public bool Compare(IResult calculationResult)
        {
            if (calculationResult == null)
            {
                return false;
            }
            return EvaluateCondition(calculationResult);
        }

        protected abstract bool EvaluateCondition(IResult calculationResult);

        #endregion

        #region object.ToString() override

        public override string ToString()
        {
            FieldInfo fieldInfo = this.GetType().GetField(RelOrAbs.ToString());
            DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            string enumDescription = "";
            if (attributes != null && attributes.Any())
            {
                enumDescription = attributes.First().Description;
            }

            return $"Name: {Name}{Environment.NewLine}Enabled: {Enabled}{Environment.NewLine}CalculationType: {CalculationType}{Environment.NewLine}{enumDescription}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {

        }

        #endregion

        public abstract XElement SaveToXml(XElement inputElement);

        public abstract bool LoadFromXml(XElement inputElement);

    }


    public abstract class ConditionBase<T> : ConditionBase, IConditionHandler<T>
        where T : struct
    {
        public T LeftValue { get; set; }


        public ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, bool enabled, Relativity relorabs)
            : base(name, calculationtype, relation, enabled, relorabs)
        {
            LeftValue = value;
        }


        public ConditionBase()
        {
            LeftValue = default(T);
        }


        // evaluation calls it from derived classes:
        protected bool Compare(T leftValue)
        {
            bool equality = EqualityComparer<T>.Default.Equals(leftValue, LeftValue);
            int compResult = Comparer<T>.Default.Compare(leftValue, LeftValue);

            switch (ConditionRelation)
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


        protected bool CheckCalculationType(IResult calculationResult, CalculationTypes calcType)
        {
            if (CalculationType != calcType)
            {
                return false;
            }

            switch (calcType)
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
                    throw new ArgumentOutOfRangeException(nameof(calcType), calcType, null);
            }
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}{LeftValue}{ConditionRelation}";
        }

        #endregion

    }
}
