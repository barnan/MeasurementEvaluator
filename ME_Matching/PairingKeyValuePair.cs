using System;
using System.Collections.Generic;

namespace MeasurementEvaluator.ME_Matching
{
    [Serializable]
    public class PairingKeyValuePair
    {

        /// <summary>
        /// Specification condition name. E.g.: Thickness Average
        /// </summary>
        public string ConditionName { get; set; }


        /// <summary>
        /// assignable measurement data names
        /// </summary>
        public List<string> MeasDataNames { get; set; }


        /// <summary>
        /// assignable reference name
        /// </summary>
        public string ReferenceName { get; set; }



        public PairingKeyValuePair()
        {
        }


        public PairingKeyValuePair(string key, List<string> values, string referenceName)
        {
            ConditionName = key;
            MeasDataNames = values;
            ReferenceName = referenceName;
        }
    }
}
