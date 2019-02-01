using Interfaces;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.ToolSpecifications
{

    public class QuantitySpecification : IQuantitySpecification
    {
        public string QuantityName { get; }
        public Units Dimension { get; }
        public IReadOnlyList<ICondition> Conditions { get; }



        public QuantitySpecification(IReadOnlyList<ICondition> conditions, Units dimension, string quantityName)
        {
            Conditions = conditions;
            Dimension = dimension;
            QuantityName = quantityName;
        }

        #region object.ToString()

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(QuantityName);
            sb.AppendLine(Dimension.ToString());

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
