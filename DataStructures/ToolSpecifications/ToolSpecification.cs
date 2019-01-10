using Interfaces;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.ToolSpecifications
{

    public class ToolSpecification : IToolSpecification, IComparable<IToolSpecification>
    {
        private readonly ToolSpecificationOnHDD _specOnHDD;

        public string FileFullPathAndName { get; }

        public List<IQuantitySpecification> Specifications => _specOnHDD.Specifications;
        public ToolNames ToolName => _specOnHDD.ToolName;


        public ToolSpecification(string fileName, ToolSpecificationOnHDD spec)
        {
            FileFullPathAndName = fileName;
            _specOnHDD = spec;
        }


        public int CompareTo(IToolSpecification other)
        {

            string toolName1 = ToolName.ToString();
            string toolName2 = other.ToolName.ToString();

            return string.Compare(toolName1, toolName2, StringComparison.InvariantCulture);
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


    }

}
