using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Calculation
{
    public abstract class CalculationBase <T, U>
    {
        public T Config { get; set; }
        public abstract void ExecuteOn(U Data);

    }
}
