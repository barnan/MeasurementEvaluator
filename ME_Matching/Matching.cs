﻿using Interfaces.Evaluation;
using Interfaces.Misc;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeasurementEvaluator.ME_Matching
{
    internal class Matching : IMathing
    {

        private readonly object _lockObj = new object();
        private readonly MathchingParameters _parameters;
        private List<MatchingKeyValuePairs> _specificationMeasDataReferencePairs;


        public Matching(MathchingParameters parameters)
        {
            _parameters = parameters;
            _parameters.Logger.Info($"{nameof(Matching)} Instantiated.");
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
                if (_parameters.MatchingFileReader == null)
                {
                    _parameters.Logger.LogError($"{nameof(_parameters.MatchingFileReader)} is null");
                    return false;
                }

                // todo: check type
                _specificationMeasDataReferencePairs = (List<MatchingKeyValuePairs>)_parameters.MatchingFileReader.ReadFromFile(_parameters.BindingFilePath);
                if (_specificationMeasDataReferencePairs == null)
                {
                    _parameters.Logger.LogError($"Deserialization of {nameof(MatchingKeyValuePairs)} was not successful from: {_parameters.BindingFilePath}");
                }

                bool oldInitState = IsInitialized;
                IsInitialized = true;
                OnInitStateChanged(IsInitialized, oldInitState);

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

                bool oldInitState = IsInitialized;
                IsInitialized = false;
                OnInitStateChanged(IsInitialized, oldInitState);

                _parameters.Logger.MethodError("Closed.");
            }
        }

        public event EventHandler<InitializationEventArgs> InitStateChanged;


        private void OnInitStateChanged(bool newState, bool oldState)
        {
            var initialized = InitStateChanged;
            initialized?.Invoke(this, new InitializationEventArgs(newState, oldState));
        }

        #endregion


        public IEnumerable<string> GetMeasDataNames(string conditionName)
        {
            lock (_lockObj)
            {
                if (!IsInitialized)
                {
                    _parameters.Logger.LogError("Not initialized yet.");
                    return null;
                }
                IEnumerable<string> result = _specificationMeasDataReferencePairs.Where(p => p.ConditionName == conditionName).SelectMany(p => p.MeasDataNames);
                return result;
            }
        }

        public string GetReferenceName(string conditionName)
        {
            lock (_lockObj)
            {
                if (!IsInitialized)
                {
                    _parameters.Logger.LogError("Not initialized yet.");
                    return null;
                }
                string result = _specificationMeasDataReferencePairs.FirstOrDefault(p => p.ConditionName == conditionName)?.ReferenceName;
                return result;
            }
        }

    }
}
