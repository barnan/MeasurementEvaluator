using Interfaces.BaseClasses;
using Interfaces.Misc;

namespace Interfaces.ToolSpecifications
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
