using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    public class ReferenceValue :IReferenceValue
    {
        public string Name { get; set; }
        public string Dimension { get; set; }
        public double Value { get; set; }

        public ReferenceValue()
        {
        }

        public ReferenceValue(string name, string dim, double val)
        {
            Name = name;
            Dimension = dim;
            Value = val;
        }

    }
}
