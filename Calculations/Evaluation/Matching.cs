using Interfaces.Evaluation;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculations.Evaluation
{
    internal class Matching : IMathing
    {

        private readonly object _lockObj = new object();
        private readonly MathchingParameters _parameters;
        private List<SimpleKeyValuePairs> _SpecificationMeasDataReferencePairs;


        // TODO: finish with xml reader


        public Matching(MathchingParameters parameters)
        {
            _parameters = parameters;

            _parameters.Logger.MethodError("Instantiated.");
        }


        #region intialized

        public bool Initiailze()
        {
            if (IsInitialized)
            {
                return true;
            }

            lock (_lockObj)
            {
                if (IsInitialized)
                {
                    return true;
                }
                //if (!_parameters.XmlReader.Initiailze())
                //{
                //    _parameters.Logger.LogError($"{nameof(_parameters.XmlReader)} could not been initialized.");
                //    return false;
                //}

                //TODO: read xml data

                _SpecificationMeasDataReferencePairs = _parameters.XmlReader.DeserializeObject<List<SimpleKeyValuePairs>>(_parameters.NameBindingFilePath);

                IsInitialized = true;

                OnInitialized();

                _parameters.Logger.MethodError("Initialized.");

                return IsInitialized;
            }
        }

        public bool IsInitialized { get; private set; }

        public void Close()
        {
            if (!IsInitialized)
            {
                return;
            }

            lock (_lockObj)
            {
                if (!IsInitialized)
                {
                    return;
                }

                IsInitialized = false;

                OnClosed();

                _parameters.Logger.MethodError("Closed.");
            }
        }

        public event EventHandler<EventArgs> Initialized;
        public event EventHandler<EventArgs> Closed;


        private void OnInitialized()
        {
            var initialized = Initialized;

            initialized?.Invoke(this, new EventArgs());
        }


        private void OnClosed()
        {
            var closed = Closed;

            closed?.Invoke(this, new EventArgs());
        }

        #endregion


        // TODO : xml reading


        public IEnumerable<string> GetMeasDataNames(string specificationName)
        {
            lock (_lockObj)
            {
                if (!IsInitialized)
                {
                    _parameters.Logger.LogError("Not initialized yet.");
                    return null;
                }
                var result = _SpecificationMeasDataReferencePairs.Where(p => p.Key.Contains(specificationName)).SelectMany(p => p.Values);
                return result;
            }
        }

        public string GetreferenceName(string specificationName)
        {
            lock (_lockObj)
            {
                if (!IsInitialized)
                {
                    _parameters.Logger.LogError("Not initialized yet.");
                    return null;
                }
                var result = _SpecificationMeasDataReferencePairs.FirstOrDefault(p => p.Key.Contains(specificationName))?.ReferenceName;
                return result;
            }
        }



    }
}
