using Interfaces.Evaluation;
using Interfaces.Misc;
using Miscellaneous;
using System.Collections.Generic;
using System.Linq;

namespace MeasurementEvaluator.ME_Matching
{
    internal class Matching : InitializableBase, IMathing
    {

        private readonly MathchingParameters _parameters;
        private List<MatchingKeyValuePairs> _specificationMeasDataReferencePairs;


        public Matching(MathchingParameters parameters)
            : base(parameters.Logger)
        {
            _parameters = parameters;
            _parameters.Logger.Info($"{nameof(Matching)} Instantiated.");
        }


        #region intialized

        protected override void InternalInit()
        {
            if (_parameters.MatchingFileReader == null)
            {
                _parameters.Logger.LogError($"{nameof(_parameters.MatchingFileReader)} is null");
                InitializationState = InitializationStates.InitializationFailed;
                return;
            }

            // todo: check type
            _specificationMeasDataReferencePairs = (List<MatchingKeyValuePairs>)_parameters.MatchingFileReader.ReadFromFile(_parameters.BindingFilePath);
            if (_specificationMeasDataReferencePairs == null)
            {
                _parameters.Logger.LogError($"Deserialization of {nameof(MatchingKeyValuePairs)} was not successful from: {_parameters.BindingFilePath}");
            }

            InitializationState = InitializationStates.Initialized;
        }

        protected override void InternalClose()
        {
            InitializationState = InitializationStates.NotInitialized;
        }

        #endregion


        public IEnumerable<string> GetMeasDataNames(string conditionName)
        {
            if (!IsInitialized)
            {
                _parameters.Logger.LogError("Not initialized yet.");
                return null;
            }
            IEnumerable<string> result = _specificationMeasDataReferencePairs.Where(p => p.ConditionName == conditionName).SelectMany(p => p.MeasDataNames);

            return result;
        }

        public string GetReferenceName(string conditionName)
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
