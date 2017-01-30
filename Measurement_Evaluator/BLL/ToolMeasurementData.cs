using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    public class ToolMeasurementData :IToolMeasurementData
    {
        public string Name { get; set; }
        public DateTime DateTimeOfMeas { get; set; }

        private List<QuantityMeasurementData> _results;
        public List<IQuantityMeasurementData> Results
        {
            get
            {
                return _results.Select(c => (IQuantityMeasurementData)c).ToList();
            }
            set
            {
                if (value == null)
                    return;

                _results.Clear();

                foreach (var item in value)
                {
                    _results.Add((QuantityMeasurementData)item);
                }
            }
        }


        public ToolMeasurementData()
        { }

        public ToolMeasurementData(string toolname)
        {
            Name = toolname;
            Results = new List<IQuantityMeasurementData>();
        }

    }
}
