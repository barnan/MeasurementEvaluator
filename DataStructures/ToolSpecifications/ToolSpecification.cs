using Interfaces;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.ToolSpecifications
{

    public class ToolSpecification : IToolSpecificationHandler
    {
        private readonly ILogger _logger;


        private List<IQuantitySpecification> _specifications;
        public IReadOnlyList<IQuantitySpecification> Specifications
        {
            get { return _specifications.AsReadOnly(); }
            set { _specifications = (List<IQuantitySpecification>)value; }
        }

        public ToolNames ToolName { get; set; }


        public string FullNameOnHDD { get; set; }



        public ToolSpecification(string fileName)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _logger.MethodInfo($"{ToolName} specification created.");
        }

        #region IFormattable

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ToolName.ToString());

            foreach (var item in Specifications)
            {
                sb.Append(item);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion

        #region Comparable

        public int CompareTo(IToolSpecification other)
        {
            try
            {
                if (ReferenceEquals(this, other))
                {
                    return 0;
                }

                if (ReferenceEquals(null, other))
                {
                    return 1;
                }

                if (other.ToolName == null)
                {
                    _logger.Error("Tool Name is null in Arrived data.");
                    return 0;
                }

                string toolName1 = ToolName.ToString();
                string toolName2 = other.ToolName.ToString();

                int toolNameComparisonResult = string.Compare(toolName1, toolName2, StringComparison.OrdinalIgnoreCase);

                if (toolNameComparisonResult != 0)
                {
                    return toolNameComparisonResult;
                }

                if (Specifications.Count != other.Specifications.Count)
                {
                    return Specifications.Count > other.Specifications.Count ? 1 : -1;
                }

                int summ = 0;
                for (int i = 0; i < Specifications.Count; i++)
                {
                    summ += Specifications[i].CompareTo(other.Specifications[i]);
                }

                if (summ != 0)
                {
                    summ /= Math.Abs(summ);
                }

                return summ;
            }
            catch (Exception ex)
            {
                _logger.MethodError($"Exception occured: {ex}");
                return 0;
            }
        }

        #endregion
    }

}
