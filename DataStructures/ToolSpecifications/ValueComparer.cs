using System;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{
    class DoubleValueComparer : IComparer<double>
    {
        private const double EQUALITY_LIMIT = 1E-7;


        public int Compare(double x, double y)
        {
            if (y > x && Math.Abs(y - x) > EQUALITY_LIMIT)
            {
                return -1;
            }

            if (y < x && Math.Abs(x - y) > EQUALITY_LIMIT)
            {
                return 1;
            }

            return 0;
        }


    }
}
