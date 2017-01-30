using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public interface IQuantitySpecification
    {
        string QuantityName { get; set; }
        string Dimension { get; }

        IUsualCondition AccurAbsolute { get; set; }
        IUsualCondition AccurRelative { get; set; }

        IUsualCondition StdAbsolute { get; set; }
        IUsualCondition StdRelative { get; set; }

        ICpkCondition Cpk { get; set; }
    }
}
