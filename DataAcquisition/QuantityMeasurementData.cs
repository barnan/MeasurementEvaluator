using System.Collections.Generic;

namespace DataAcquisition
{
    class QuantityMeasurementData : IQuantityMeasurementData
    {
        public string Name { get; set; }
        public List<double> MeasData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public QuantityMeasurementData()
        {
            MeasData = new List<double>();
        }
    }
}
