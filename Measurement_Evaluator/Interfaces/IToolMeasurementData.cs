using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public interface IToolMeasurementData
    {
        string Name { get; set; }
        DateTime DateTimeOfMeas { get; set; }
        List<IQuantityMeasurementData> Results { get; set; }

    }
}
