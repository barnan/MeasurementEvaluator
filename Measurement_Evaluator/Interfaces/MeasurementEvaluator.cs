using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public enum MeasurementEvaluatorSystemMode
    {
        SkewCalculation,
        SimpleEvalution,
        ConfigEdit
    }

    public enum MeasurementEvaluatorSystemSate
    {
        Ready,
        NotReady,
        Calculating
    }



    interface MeasurementEvaluator
    {
    }
}
