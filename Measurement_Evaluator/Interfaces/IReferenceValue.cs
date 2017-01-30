using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public interface IReferenceValue
    {
        string Name { get; set; }
        string Dimension { get; set; }
        double Value { get; set; }

    }
}
