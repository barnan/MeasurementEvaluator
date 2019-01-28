using System.Collections.Generic;

namespace Calculations.Evaluation
{
    public class SimpleKeyValuePairs
    {

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; }


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Values { get; }


        public SimpleKeyValuePairs()
        {
        }


        public SimpleKeyValuePairs(string key, List<string> values)
        {
            Key = key;
            Values = values;
        }

    }
}
