namespace MeasurementEvaluatorUIWPF.UserControls.EvaluationTab
{
    class EvaluationTabViewModel
    {
        private EvaluationTabUIWPFParameters _parameters;
        public EvaluationTabUIWPFParameters Parameters => _parameters;



        public EvaluationTabViewModel(EvaluationTabUIWPFParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
