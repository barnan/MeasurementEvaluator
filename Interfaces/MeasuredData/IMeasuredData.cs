using System.Collections.Generic;

namespace Measurement_Evaluator.Interfaces
{
    public interface IMeasuredData
    {
        string Name { get; set; }
        List<double> MeasData { get; set; }
    }
}
