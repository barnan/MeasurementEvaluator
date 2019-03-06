using Interfaces.ToolSpecifications;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.ToolSpecifications
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


        #region IComparable

        public int CompareTo(IQuantitySpecification other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            if (other.Conditions.Count == Conditions.Count)
            {
                return 0;
            }

            return Conditions.Count > other.Conditions.Count ? 1 : -1;
        }

        #endregion

    }
}
