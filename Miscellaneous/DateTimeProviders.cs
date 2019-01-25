using Interfaces.Misc;
using System;

namespace Miscellaneous
{
    public sealed class StandardDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public StandardDateTimeProvider(StandardDateTimeProviderParameter parameter)
        {
        }
    }

    public sealed class StandardDateTimeProviderParameter
    {

    }




    public sealed class SimulatedDateTimeProvider : IDateTimeProvider
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


    public sealed class SimulatedDateTimeProviderParameter
    {
        public DateTime DateTimeToUse { get; }
    }

}
