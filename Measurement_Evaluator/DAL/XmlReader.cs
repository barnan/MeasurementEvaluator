using Measurement_Evaluator.BLL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    class XmlReader : MeasDataFileBase
    {
        public XmlReader(string filename, string toolname) 
            : base(filename, toolname)
        {
        }

        public override IToolMeasurementData ReadFile()
        {
            // TODO xml reading





            return new ToolMeasurementData();


        }
    }
}
