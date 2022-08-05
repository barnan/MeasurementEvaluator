using System.Xml.Linq;
using BaseClasses.MeasurementEvaluator;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace Evaluation.DataStructures.ToolSpecification
{
    internal abstract class ConditionBase : IConditionHandler
    {
        protected ConditionBase(string name, CalculationTypes calculationtype, Relations relation, bool enabled)
        {
            Name = name;
            CalculationType = calculationtype;
            Relation = relation;
            Enabled = enabled;
        }

        public ConditionBase()
            : this(string.Empty, CalculationTypes.Unknown, Relations.EQUAL, false)
        {
        }


        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
            return $"Name: {Name}{Environment.NewLine}Enabled: {Enabled}{Environment.NewLine}CalculationType: {CalculationType}";
        }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
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



    internal abstract class ConditionBase<T> : ConditionBase, IConditionHandler<T>
        where T : struct
    {

        public T LeftValue { get; set; }


        #region ctor

        protected ConditionBase(string name, CalculationTypes calculationtype, T value, Relations relation, bool enabled)
            : base(name, calculationtype, relation, enabled)
        {
            LeftValue = value;
        }


        protected ConditionBase() 
        {
            LeftValue = default(T);
        }

        #endregion

        #region object.ToString()

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}{LeftValue}{Relation}";
        }

        #endregion

    }
}
