using System.Text;
using System.Xml.Linq;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification
{
    public class QuantitySpecification : IQuantitySpecificationHandler
    {
        #region ctor

        public QuantitySpecification(IReadOnlyList<ICondition> conditions, IQuantity quantity)
        {
            Conditions = conditions;
            Quantity = quantity;
        }

        #endregion


        #region IQuantitySpecification

        public IReadOnlyList<ICondition> Conditions { get; set; }

        public IQuantity Quantity { get; set; }

        #endregion


        #region object.ToString()

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Quantity.Name);
            sb.AppendLine(Quantity.Dimension.ToString());

            foreach (var item in Conditions)
            {
                sb.Append(item);
            }

            return sb.ToString();
        }

        #endregion

        #region IXmlStorable

        public XElement SaveToXml(XElement inputElement)
        {
            //this.TrySave(Conditions, inputElement, nameof(Conditions));
            //this.TrySave(Quantity, inputElement, nameof(Quantity));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            //this.TryLoad(inputElement, nameof(Conditions));
            //this.TryLoad(inputElement, nameof(Quantity));
            return true;
        }

        #endregion

    }

}
