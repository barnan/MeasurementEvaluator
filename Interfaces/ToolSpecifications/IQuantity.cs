using Interfaces.Misc;

namespace Interfaces.ToolSpecifications
{
    public interface IQuantity : INamed
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
