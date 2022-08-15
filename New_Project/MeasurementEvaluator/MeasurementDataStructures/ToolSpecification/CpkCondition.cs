using System.Xml.Linq;
using Interfaces;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification
{
    public class CpkCondition : ConditionBase, ICpkConditionHandler
    {
        public double HalfTolerance { get; set; }


        public CpkCondition(string name, CalculationTypes calculationtype, Relations relation, Relativities relativity, bool enabled, double halfTolerance)
            : base(name, calculationtype, relation, enabled)
        {
            HalfTolerance = halfTolerance;
        }


        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}HalfTolerance: {HalfTolerance}";
        }

        #endregion

        
        #region XmlStorable

        public override XElement SaveToXml(XElement inputElement)
        {
            throw new NotImplementedException();

            //this.TrySave(Name, inputElement, nameof(Name));
            //this.TrySave(Enabled, inputElement, nameof(Enabled));
            //this.TrySave(CalculationTypeHandler, inputElement, nameof(CalculationTypeHandler));
            //this.TrySave(ConditionRelation, inputElement, nameof(ConditionRelation));
            //this.TrySave(LeftValue, inputElement, nameof(LeftValue));
            //this.TrySave(HalfTolerance, inputElement, nameof(HalfTolerance));

            return inputElement;
        }

        public override bool LoadFromXml(XElement inputElement)
        {
            throw new NotImplementedException();

            //this.TryLoad(inputElement, nameof(Name));
            //this.TryLoad(inputElement, nameof(Enabled));
            //this.TryLoad(inputElement, nameof(CalculationTypeHandler));
            //this.TryLoad(inputElement, nameof(ConditionRelation));
            //this.TryLoad(inputElement, nameof(LeftValue));
            //this.TryLoad(inputElement, nameof(HalfTolerance));
            return true;
        }

        #endregion
    }
}
