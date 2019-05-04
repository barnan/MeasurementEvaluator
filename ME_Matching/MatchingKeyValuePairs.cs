using System;
using System.Collections.Generic;

namespace MeasurementEvaluator.ME_Matching
{
    [Serializable]
    public class MatchingKeyValuePairs
    {

        /// <summary>
        /// Specification condition name. E.g.: Thickness Average
        /// </summary>
        public string ConditionName { get; }


        /// <summary>
        /// assignable measurement data names
        /// </summary>
        public IList<string> MeasDataNames { get; }


        /// <summary>
        /// assignable reference name
        /// </summary>
        public string ReferenceName { get; }



        public MatchingKeyValuePairs()
        {
        }


        public MatchingKeyValuePairs(string key, IList<string> values, string referenceName)
        {
            ConditionName = key;
            MeasDataNames = values;
            ReferenceName = referenceName;
        }
    }
}
