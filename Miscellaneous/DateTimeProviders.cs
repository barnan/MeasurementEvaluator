using Interfaces.Misc;
using System;

namespace Miscellaneous
{
    public class StandardDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public StandardDateTimeProvider(StandardDateTimeProviderParameter parameter)
        {
        }
    }

    public class StandardDateTimeProviderParameter
    {

    }




    public class SimulatedDateTimeProvider : IDateTimeProvider
    {
        SimulatedDateTimeProviderParameter _parameter;


        public DateTime GetDateTime()
        {
            return _parameter.DateTimeToUse;
        }


        public SimulatedDateTimeProvider(SimulatedDateTimeProviderParameter parameter)
        {
            _parameter = parameter;
        }
    }


    public class SimulatedDateTimeProviderParameter
    {
        public DateTime DateTimeToUse { get; }
    }

}
