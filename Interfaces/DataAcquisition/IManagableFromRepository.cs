using Interfaces.Misc;
using System;

namespace Interfaces.DataAcquisition
{
    public interface IManagableFromRepository<T> : IComparable<T>, INamed
    {
    }
}
