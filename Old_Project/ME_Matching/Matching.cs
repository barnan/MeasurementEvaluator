﻿using Frame.MessageHandler;
using Frame.PluginLoader;
using Interfaces.Evaluation;
using Interfaces.Misc;
using Miscellaneous;
using System.Collections.Generic;
using System.IO;
using System.Linq;


//[assembly: InternalsVisibleTo("Test_Matcher")]

namespace MeasurementEvaluator.ME_Matching
{

    internal class Matching : InitializableBase, IMatching
    {

        private readonly PairingParameters _parameters;
        private List<PairingKeyValuePair> _specificationMeasDataReferencePairs;


        public Matching(PairingParameters parameters)
            : base(parameters.Logger)
        {
            _parameters = parameters;
            _parameters.Logger.Info($"{nameof(Matching)} Instantiated.");
        }


        #region intialized

        protected override void InternalInit()
        {
            if (_parameters.PairingFileReader == null)
            {
                HandleInitializationFailed($"{nameof(_parameters.PairingFileReader)} is null");
                return;
            }

            // todo: check type
            string fullFilePath = Path.Combine(PluginLoader.SpecificationFolder, _parameters.BindingFilePath);

            _specificationMeasDataReferencePairs = (List<PairingKeyValuePair>)_parameters.PairingFileReader.ReadFromFile(fullFilePath, typeof(List<PairingKeyValuePair>));
            if (_specificationMeasDataReferencePairs == null)
            {
                _parameters.Logger.LogError($"Deserialization of {nameof(PairingKeyValuePair)} was not successful from: {_parameters.BindingFilePath}");
            }

            InitializationState = InitializationStates.Initialized;

            _parameters.MessageControl.AddMessage("Matching initialized.");
        }

        private void HandleInitializationFailed(string message)
        {
            _parameters.MessageControl.AddMessage(_parameters.Logger.LogError(message), MessageSeverityLevels.Error);
            InitializationState = InitializationStates.InitializationFailed;
        }


        protected override void InternalClose()
        {
            InitializationState = InitializationStates.NotInitialized;
        }

        #endregion


        public IEnumerable<string> GetMeasDataNames(string searchedConditionName)
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return null;
            }
            IEnumerable<string> result = _specificationMeasDataReferencePairs.Where(p => p.ConditionName == searchedConditionName).SelectMany(p => p.MeasDataNames);

            return result;
        }

        public string GetReferenceName(string searchedConditionName)
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return null;
            }
            string result = _specificationMeasDataReferencePairs.FirstOrDefault(p => p.ConditionName == searchedConditionName)?.ReferenceName;
            return result;
        }

    }
}
