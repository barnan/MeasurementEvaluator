using Interfaces.MeasuredData;
using Measurement_Evaluator.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAcquisition
{
    public class ToolMeasurementData : IToolMeasurementData
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
        }


        public IQuantityMeasurementData this[int i]
        {
            get
            {
                if (i < 0 && i > _results.Count - 1)
                    throw new ArgumentException("Not proper parameter in ToolMeasurementData indexer-get");

                return _results[i];
            }
            set
            {
                if (i < 0 && i > _results.Count - 1)
                    throw new ArgumentException("Not proper parameter in ToolMeasurementData indexer-set");

                QuantityMeasurementData qmd = value as QuantityMeasurementData;
                _results[i] = qmd;
            }
        }


        public void Add(IQuantityMeasurementData data)
        {
            QuantityMeasurementData qmd = data as QuantityMeasurementData;

            if (qmd == null)
                throw new ArgumentException("Not proper input valriable in ToolMeasurementData.Add()");

            _results.Add(qmd);
        }


        public ToolMeasurementData()
        {
            _results = new List<QuantityMeasurementData>();
            //Results = new List<IQuantityMeasurementData>();
        }

        public ToolMeasurementData(string toolname)
            : this()
        {
            Name = toolname;
        }

    }


}
