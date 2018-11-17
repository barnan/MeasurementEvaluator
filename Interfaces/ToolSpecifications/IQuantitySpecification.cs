using System.Collections.Generic;

namespace Interfaces.ToolSpecifications
{
    public interface IQuantitySpecification
    {
        string QuantityName { get; set; }
        Units Dimension { get; }

        IReadOnlyList<ICondition> Conditions { get; set; }

        //ISimpleCondition AccurAbsolute { get; set; }
        //ISimpleCondition AccurRelative { get; set; }
        //ISimpleCondition StdAbsolute { get; set; }
        //ISimpleCondition StdRelative { get; set; }
        //ICpkCondition Cpk { get; set; }
    }
}
