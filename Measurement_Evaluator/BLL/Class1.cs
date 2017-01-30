using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.DAL;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    class Class1
    {


        public static void Fut()
        {

            UsualCondition uc1 = new UsualCondition(11, "um", Interfaces.Relation.LESS, true, Interfaces.Relation.LESSOREQUAL, 101);
            UsualCondition uc2 = new UsualCondition(12, "um", Interfaces.Relation.GREATER, true, Interfaces.Relation.LESSOREQUAL, 102);
            UsualCondition uc3 = new UsualCondition(13, "um", Interfaces.Relation.EQUAL, true, Interfaces.Relation.LESSOREQUAL, 103);
            UsualCondition uc4 = new UsualCondition(14, "um", Interfaces.Relation.LESSOREQUAL, true, Interfaces.Relation.LESSOREQUAL, 104);


            CpkCondition cpk = new CpkCondition(20, Interfaces.Relation.LESSOREQUAL, Interfaces.RELATIVEORABSOLUTE.ABSOLUTE, true, 7);
            QuantitySpecification qs = new QuantitySpecification("thickness", uc1, uc2, uc3, uc4, cpk);

            List<IQuantitySpecification> iq = new List<IQuantitySpecification>();
            iq.Add(qs);

            ToolSpecification ts = new ToolSpecification("TTR", iq );
            ToolSpecification.Save("proba4.xml", ts);


            ReferenceValue refVal = new ReferenceValue("Thickness", "um", 200);
            List<IReferenceValue> refVals = new List<IReferenceValue>();
            refVals.Add(refVal);

            ReferenceSample refSample = new ReferenceSample("TTR-001", refVals, Orientation.Orientation1);
            ReferenceSample.Save("proba10.xml", refSample);

        }

    }
}
