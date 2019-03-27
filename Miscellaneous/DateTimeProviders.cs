using Interfaces.Misc;
using System;

namespace Miscellaneous
{
    internal sealed class StandardDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public StandardDateTimeProvider(StandardDateTimeProviderParameter parameter)
        {
        }
    }

    internal sealed class StandardDateTimeProviderParameter
    {
    }




    internal sealed class SimulatedDateTimeProvider : IDateTimeProvider
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


    internal sealed class SimulatedDateTimeProviderParameter
    {
        public DateTime DateTimeToUse { get; }
    }

}
