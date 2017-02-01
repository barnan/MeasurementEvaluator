using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public interface IMeasurementDataReader
    {
        IToolMeasurementData ReadMeasurementData(List<string> inputs);
    }
}
