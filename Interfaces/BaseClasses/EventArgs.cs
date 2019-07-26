using Interfaces.Result;
using System;
using System.Collections.Generic;

namespace Interfaces.BaseClasses
{

    public class ResultEventArgs : EventArgs
    {
        public IResult Result { get; }
        public ResultEventArgs(IResult result)
        {
            Result = result;
        }
    }



    public class DataCollectorResultEventArgs : EventArgs
    {
        List<string> SpecificationName { get; }
        List<string> MeasurementDataFileNames { get; }
        List<string> ReferenceName { get; }

        public DataCollectorResultEventArgs(List<string> specificationName, List<string> measurementDataFileNames, List<string> referenceName)
        {
            SpecificationName = specificationName;
            MeasurementDataFileNames = measurementDataFileNames;
            ReferenceName = referenceName;
        }
    }



    public class CustomEventArg<T> : EventArgs
    {
        private readonly T _data;

        public CustomEventArg(T data)
        {
            _data = data;
        }

        public T Data => _data;
    }
}
