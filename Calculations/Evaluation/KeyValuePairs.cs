using System.Collections.Generic;

namespace Calculations.Evaluation
{
    public class KeyValuePairs
    {

        public string Key { get; set; }


        public IEnumerable<string> Values { get; set; }


        public KeyValuePairs()
        {
        }


        public KeyValuePairs(string key, List<string> values)
        {
            Key = key;
            Values = values;
        }

    }
}
