using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.BLL
{
    
    public class ConditionBase :IConditionBase
    {
        public double Value { get; set; }
        public Relation ConditionRel { get; set; }
        public bool Valid { get; set; }

        public ConditionBase()
        {
        }

        public ConditionBase(double value, Relation relat, bool valid)
        {
            Value = value;
            ConditionRel = relat;
            Valid = valid;
        }

        public override string ToString()
        {
            if (Valid)
                return "Valid. It should be " + ConditionRel.ToString() + " than " + Value.ToString();
            else
                return "NOT Valid. It should be " + ConditionRel.ToString() + " than " + Value.ToString();
        }

        public static bool Compare(double value1, Relation condrel, double value2)
        {
            switch (condrel)
            {
                case Relation.LESS:
                    return (value1 < value2);
                case Relation.GREATER:
                    return (value1 > value2);
                case Relation.LESSOREQUAL:
                    return (value1 <= value2);
                case Relation.GREATEROREQUAL:
                    return (value1 >= value2);
                case Relation.EQUAL:
                    return (value1 == value2);
                case Relation.NOTEQUAL:
                    return (value1 != value2);
            }
            return false;
        }

    }
}
