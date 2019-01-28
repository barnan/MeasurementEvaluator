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
        private List<SimpleKeyValuePairs> _measDataSpecificationPairs;
        private List<SimpleKeyValuePairs> _measDataReferencePairs;


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

                _measDataSpecificationPairs = _parameters.XmlReader.DeserializeObject<List<SimpleKeyValuePairs>>(_parameters.MeasurementDataSpecificationFilePath);
                _measDataReferencePairs = _parameters.XmlReader.DeserializeObject<List<SimpleKeyValuePairs>>(_parameters.MeasurementDataReferenceFilePath);

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


        // TODO: xml reading
        // TODO : szálvédelem
        public string GetSpecification(string measuredDataName)
        {
            var result = _measDataSpecificationPairs.Where(p => p.Values.Contains(measuredDataName)).Select(p => p.Key).ToList();

            if (result.Count == 0)
            {
                _parameters.Logger.LogError("No matching specification was found.");
                return null;
            }

            if (result.Count > 1)
            {
                _parameters.Logger.LogError("More than 1 matching specification was found -> probably wrong matching xml");
                return null;
            }

            return result[0];
        }

        public string Getreference(string measuredDataName)
        {
            var result = _measDataReferencePairs.Where(p => p.Key == measuredDataName).Select(p => p.Key).ToList();
            if (result.Count == 0)
            {
                _parameters.Logger.LogError("No matching reference was found.");
                return null;
            }

            if (result.Count > 1)
            {
                _parameters.Logger.LogError("More than 1 matching reference was found -> probably wrong matching xml");
                return null;
            }

            return result[0];
        }


        #endregion

    }
}
