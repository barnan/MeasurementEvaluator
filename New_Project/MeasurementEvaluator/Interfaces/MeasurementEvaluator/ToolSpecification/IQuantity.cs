using BaseClasses.MeasurementEvaluator;
using Interfaces.Misc;

namespace Interfaces.MeasurementEvaluator.ToolSpecification
{
    public interface IQuantity : INamed, IXmlStorable
    {
        /// <summary>
        /// unit of quantity
        /// </summary>
        Units Dimension { get; }
    }



    public interface IQuantityHandler : IQuantity, INamedHandler
    {
        new Units Dimension { get; set; }
    }
}
