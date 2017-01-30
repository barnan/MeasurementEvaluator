using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    public enum Orientation { Orientation1 = 0, Orientation2 = 90, Orientation3 = 270, Orientation4 = 360 };

    interface IReferenceSample
    {
        string SampleID { get; set; }
        List<IReferenceValue> ListOfReferenceValues { get; set; }
        Orientation SampleOrientation { get; set; }
    }
}
