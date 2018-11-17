using Measurement_Evaluator.Interfaces.ReferenceSample;
using System.Collections.Generic;

namespace Measurement_Evaluator.Interfaces.ToolSpecifications
{
    public interface IQuantitySpecification
    {
        string QuantityName { get; set; }
        Dimensions Dimension { get; }


        IReadOnlyList<ICondition> Conditions { get; set; }


        ISimpleCondition AccurAbsolute { get; set; }
        ISimpleCondition AccurRelative { get; set; }

        ISimpleCondition StdAbsolute { get; set; }
        ISimpleCondition StdRelative { get; set; }

        ICpkCondition Cpk { get; set; }
    }
}
