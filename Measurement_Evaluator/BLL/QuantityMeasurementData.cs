using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    class QuantityMeasurementData :IQuantityMeasurementData
    {
        public string Name { get; set; }
        public List<double> MeasData { get; set; }
    }
}
