﻿using Interfaces.Misc;
using System;

namespace Interfaces.ReferenceSample
{

    public interface IReferenceValue : IComparable<IReferenceValue>, INamedObject
    {
        /// <summary>
        /// Dimension of the Value
        /// </summary>
        Units Dimension { get; }
    }

    public interface IReferenceValue<T> : IReferenceValue where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        T Value { get; }
    }



    public interface IReferenceValueHandler : IReferenceValue, INamedObjectHandler
    {
        new Units Dimension { get; set; }
    }

    public interface IReferenceValueHandler<T> : IReferenceValueHandler, IReferenceValue<T> where T : struct
    {
        /// <summary>
        /// number value
        /// </summary>
        new T Value { get; set; }
    }


}
