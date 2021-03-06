﻿using DataStructures.ToolSpecifications.Results;
using Interfaces.BaseClasses;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace DataStructures.ToolSpecifications
{
    public class CpkCondition : ConditionBase<double>, ICpkConditionHandler
    {
        public double HalfTolerance { get; set; }


        public CpkCondition()
            : base()
        {
            HalfTolerance = 0;
        }


        public CpkCondition(string name, CalculationTypeHandler calculationtype, double value, RelationHandler relation, Relativity relativity, bool enabled, double halfTolerance)
            : base(name, calculationtype, value, relation, enabled)
        {
            HalfTolerance = halfTolerance;
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}HalfTolerance: {HalfTolerance}";
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
                    return $"{LeftValue.ToString(format, formatProvider)} {ConditionRelation}";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }

        #endregion

        #region protected

        protected override IConditionEvaluationResult InternalEvaluate(IResult result, DateTime dateTime, IReferenceValue referenceValue)
        {
            IQCellsCalculationResult qcellsResult = result as IQCellsCalculationResult;

            bool isMet = EvaluateRelation(qcellsResult.ResultValue);

            return new ConditionEvaluationResultBase(dateTime, this, referenceValue, isMet, qcellsResult);
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
            this.TrySave(HalfTolerance, inputElement, nameof(HalfTolerance));

            return inputElement;
        }

        public override bool LoadFromXml(XElement inputElement)
        {
            this.TryLoad(inputElement, nameof(Name));
            this.TryLoad(inputElement, nameof(Enabled));
            this.TryLoad(inputElement, nameof(CalculationTypeHandler));
            this.TryLoad(inputElement, nameof(ConditionRelation));
            this.TryLoad(inputElement, nameof(LeftValue));
            this.TryLoad(inputElement, nameof(HalfTolerance));
            return true;
        }

        #endregion
    }

}
