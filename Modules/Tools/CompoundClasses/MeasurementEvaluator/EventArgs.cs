using System;

namespace Interfaces.BaseClasses
{




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
