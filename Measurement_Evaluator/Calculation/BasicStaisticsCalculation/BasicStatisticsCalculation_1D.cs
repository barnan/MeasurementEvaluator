using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Calculation.StaisticsCalculation
{
    public class BasicStatisticsCalculation_1D : BasicStatisticsCalculationBase_1D
    {
        public override void ExecuteOn(IBasicStatisticsCalculation_1D_Data Data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double CalcMeanAbsolute_1D(double [] input)
        {
            return input.Where(p => p != 0).Average();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mean"></param>
        /// <returns></returns>
        public double CalcStdAbsolute_1D(double[] input, double mean)
        {
            double sumOfSquaresOfDiff = input.Where(x => x != 0).Select(val => (val - mean) * (val - mean)).Sum();

            return Math.Sqrt(sumOfSquaresOfDiff / (input.Count(x => x != 0) - 1));          // normalized by (N-1)
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double CalcMaxAbsolute_1D(double[] input)
        {
            return input.Where(p => p != 0).Max();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double CalcMinAbsolute_1D(double[] input)
        {
            return input.Where(x => x != 0).Min();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double CalcMeanRelative_1D(double[] input)
        {
            return input.Where(p => p != 0).Average();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public double CalcStdRelative_1D(double[] input)
        {
            return input.Where(p => p != 0).Average();
        }


    }
}
