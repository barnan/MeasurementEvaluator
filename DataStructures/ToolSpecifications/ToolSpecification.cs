using Interfaces;
using Interfaces.ToolSpecifications;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.ToolSpecifications
{

    public class ToolSpecification : IToolSpecification, IComparable<IToolSpecification>
    {
        private readonly ToolSpecificationOnHDD _specOnHDD;
        private readonly ILogger _logger;



        public string FullNameOnStorage { get; }


        public IReadOnlyList<IQuantitySpecification> Specifications => _specOnHDD.Specifications.AsReadOnly();


        public ToolNames ToolName => _specOnHDD.ToolName;


        public ToolSpecification(string fileName, ToolSpecificationOnHDD spec, ILogger logger)
        {
            FullNameOnStorage = fileName;
            _logger = logger;

            _specOnHDD = spec;
        }


        public int CompareTo(IToolSpecification other)
        {

            //string toolName1 = ToolName.ToString();
            //string toolName2 = other.ToolName.ToString();

            //return string.Compare(toolName1, toolName2, StringComparison.InvariantCulture);
            return 0;
        }


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


        public int Compare(IToolSpecification x, IToolSpecification y)
        {
            if (x?.ToolName == null || y?.ToolName == null)
            {
                _logger.Error("Arrived data is null.");
                throw new ArgumentNullException();
            }


            string toolName1 = x.ToolName.ToString();
            string toolName2 = y.ToolName.ToString();

            int toolNameComparisonResult = string.Compare(toolName1, toolName2, StringComparison.InvariantCulture);

            // TODO : finish
        }
    }

}
