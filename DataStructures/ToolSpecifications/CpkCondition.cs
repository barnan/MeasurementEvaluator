using Interfaces;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
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


        public CpkCondition(string name, CalculationTypes calculationtype, double value, Relations relation, bool enabled, Relativity relativity, double halfTolerance)
            : base(name, calculationtype, value, relation, enabled, relativity)
        {
            HalfTolerance = halfTolerance;
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}HalfTolerance: {HalfTolerance}{Environment.NewLine}{RelOrAbs}";
        }

        #endregion

        #region protected

        protected override bool EvaluateCondition(ICalculationResult calculationResult)
        {
            if (!CheckCalculationType(calculationResult, CalculationType))
            {
                return false;
            }

            if (!(calculationResult is IQCellsCalculationResult qcellsResult))
            {
                return false;
            }

            return Compare(qcellsResult.Result);
        }

        #endregion


        public override XElement SaveToXml(XElement inputElement)
        {
            XElement simpleConditionNode = new XElement(nameof(CpkCondition));
            simpleConditionNode.SetAttributeValue(nameof(Name), Name);
            simpleConditionNode.Add(new XElement(nameof(CalculationType), CalculationType));
            simpleConditionNode.Add(new XElement(nameof(ConditionRelation), ConditionRelation));
            simpleConditionNode.Add(new XElement(nameof(RelOrAbs), RelOrAbs));
            simpleConditionNode.Add(new XElement(nameof(Value), Value));
            simpleConditionNode.Add(new XElement(nameof(HalfTolerance), HalfTolerance));

            inputElement.Add(simpleConditionNode);
            return inputElement;
        }

        public override bool LoadFromXml(XElement inputElement)
        {
            throw new NotImplementedException();
        }
    }

}
