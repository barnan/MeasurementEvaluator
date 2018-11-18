using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Calculation.StaisticsCalculation
{
    public interface IBasicStatisticsCalculation_1D_Data
    {
        double MeanAbsolute_1D { get; set; }
        double StdAbsolute_1D { get; set; }
        double MaxAbsolute_1D { get; set; }
        double MinAbsolute_1D { get; set; }
        double ModeAbsolute_1D { get; set; }

        double StdRelative_1D { get; set; }
        double MeanRelative_1D { get; set; }
    }
}
