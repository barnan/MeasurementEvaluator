using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    interface EvaluatorFrame
    {
        void Initialize();

        IDataReader DataReader { get; set; }

        ICalculator Calculator { get; set; }

        IEvaluator Evaluator { get; set; }


    }
}
