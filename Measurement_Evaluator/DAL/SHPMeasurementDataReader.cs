using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    public enum SHPMeasDataFilextensions
    {
        [Description(".csv")]
        csv,
        [Description(".xml")]
        xml
    }


    class SHPMeasurementDataReader : IMeasurementDataReader
    {




        public IToolMeasurementData ReadMeasurementData(List<string> inputs)
        {

            return new ToolMeasurementData();


        }
    }
}
