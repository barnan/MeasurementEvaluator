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
        private List<KeyValuePairs> _measDataSpecificationPairs;
        private List<KeyValuePairs> _measDataReferencePairs;


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

                //_measDataSpecificationPairs = new List<KeyValuePairs>();
                //_measDataReferencePairs = new List<KeyValuePairs>();

                //TODO: read xml data

                _measDataSpecificationPairs = _parameters.XmlReader.DeserializeObject<List<KeyValuePairs>>(_parameters.MeasurementDataSpecificationFilePath);
                _measDataReferencePairs = _parameters.XmlReader.DeserializeObject<List<KeyValuePairs>>(_parameters.MeasurementDataReferenceFilePath);

                IsInitialized = true;

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

                _parameters.Logger.MethodError("Closed.");
            }
        }

        public event EventHandler<EventArgs> Initialized;
        public event EventHandler<EventArgs> Closed;


        // TODO : szálvédelem
        public IEnumerable<string> GetSpecification(string measuredDataName)
        {

            var result = _measDataSpecificationPairs.Where(p => p.Key == measuredDataName).SelectMany(p => p.Values);

            return result;
        }

        public IEnumerable<string> Getreference(string measuredDataName)
        {
            var result = _measDataReferencePairs.Where(p => p.Key == measuredDataName).SelectMany(p => p.Values);


            return result;
        }


        #endregion

    }
}
