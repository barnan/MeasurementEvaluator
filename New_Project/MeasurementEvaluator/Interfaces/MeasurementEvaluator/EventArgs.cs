
using Interfaces.MeasurementEvaluator.Result;

namespace Interfaces.MeasurementEvaluator
{
    public class ResultEventArgs : EventArgs
    {
        public IResult Result { get; }

        public ResultEventArgs(IResult result)
        {
            Result = result;
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
