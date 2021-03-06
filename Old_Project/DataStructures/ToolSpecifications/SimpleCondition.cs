﻿using DataStructures.ToolSpecifications.Results;
using Interfaces.BaseClasses;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{
    public class SimpleCondition : ConditionBase<double>, ISimpleConditionHandler
    {

        public RelationHandler ValidIf { get; set; }

        public double ValidIf_Value { get; set; }


        #region ctor

        public SimpleCondition()
        {
        }


        public SimpleCondition(string name, CalculationTypeHandler calculationtype, double value, RelationHandler relation, bool enabled, RelationHandler validIf, double validIfValue)
            : base(name, calculationtype, value, relation, enabled)
        {
            ValidIf = validIf;
            ValidIf_Value = validIfValue;
        }

        #endregion

        #region ICondition

        protected override IConditionEvaluationResult InternalEvaluate(IResult result, DateTime dateTime, IReferenceValue referenceValue)
        {
            ISimpleCalculationResult calculationResult = result as ISimpleCalculationResult;

            return CompareValidity(calculationResult.Average) ?
                new ConditionEvaluationResultBase(dateTime,
                    this,
                    referenceValue,
                    EvaluateRelation(calculationResult.ResultValue),
                    calculationResult)
                : null;
        }


        // evaluation calls it from derived classes:
        private bool CompareValidity(double rightValue)
        {
            bool equality = EqualityComparer<double>.Default.Equals(ValidIf_Value, rightValue);
            int compResult = Comparer<double>.Default.Compare(ValidIf_Value, rightValue);

            return ValidIf.Evaluation(equality, compResult);
        }

        #endregion

        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}Valid, if {ValidIf} than {ValidIf_Value}";
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            switch (format.ToUpperInvariant())
            {
                case null:
                case "":
                case "G":
                    return ToString();
                case "GRID":
                    return $"{LeftValue.ToString("N2")} {ConditionRelation}";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }

        #endregion


        #region XmlStorable

        public override XElement SaveToXml(XElement inputElement)
        {
            this.TrySave(Name, inputElement, nameof(Name));
            this.TrySave(Enabled, inputElement, nameof(Enabled));
            this.TrySave(CalculationTypeHandler, inputElement, nameof(CalculationTypeHandler));
            this.TrySave(ConditionRelation, inputElement, nameof(ConditionRelation));
            this.TrySave(LeftValue, inputElement, nameof(LeftValue));
            this.TrySave(ValidIf, inputElement, nameof(ValidIf));
            this.TrySave(ValidIf_Value, inputElement, nameof(ValidIf_Value));

            return inputElement;
        }

        public override bool LoadFromXml(XElement inputElement)
        {
            this.TryLoad(inputElement, nameof(Name));
            this.TryLoad(inputElement, nameof(Enabled));
            this.TryLoad(inputElement, nameof(CalculationTypeHandler));
            this.TryLoad(inputElement, nameof(ConditionRelation));
            this.TryLoad(inputElement, nameof(LeftValue));
            this.TryLoad(inputElement, nameof(ValidIf));
            this.TryLoad(inputElement, nameof(ValidIf_Value));
            return true;
        }

        #endregion

    }
}
