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
        List<IMeasuredData> Results { get; }
        void Add(IMeasuredData data);
        IMeasuredData this[int i] { get; set; }
    }

}
