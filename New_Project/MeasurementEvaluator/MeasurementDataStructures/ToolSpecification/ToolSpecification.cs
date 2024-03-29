﻿using System.Text;
using System.Xml.Linq;
using Interfaces;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification
{
    public class ToolSpecification : IToolSpecificationHandler
    {

        #region IToolspecificationHandler

        private List<IQuantitySpecification> _specifications;
        public IReadOnlyList<IQuantitySpecification> QuantitySpecifications
        {
            get => _specifications.AsReadOnly();
            set => _specifications = (List<IQuantitySpecification>)value;
        }

        public ToolNames ToolName { get; set; }

        public string Name { get; set; }

        #endregion

        #region object.toString()

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Tool: {ToolName}");

            foreach (var item in QuantitySpecifications)
            {
                sb.Append(item);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion

        public XElement SaveToXml(XElement inputElement)
        {
            //this.TrySave(Name, inputElement, nameof(Name));
            //this.TrySave(ToolName, inputElement, nameof(ToolName));
            //this.TrySave(QuantitySpecifications, inputElement, nameof(QuantitySpecifications));
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            //this.TryLoad(inputElement, nameof(Name));
            //this.TryLoad(inputElement, nameof(ToolName));
            //this.TryLoad(inputElement, nameof(QuantitySpecifications));
            return true;
        }
    }
}
