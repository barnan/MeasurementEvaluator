namespace MeasurementEvaluatorUIWPF.Pages.EvaluationPage2
{
    class EvaluationUserControlViewModel
    {
        private EvaluationUserControlUIWPFParameters _parameters;
        public EvaluationUserControlUIWPFParameters Parameters => _parameters;



        public EvaluationUserControlViewModel(EvaluationUserControlUIWPFParameters parameters)
        {
            _parameters = parameters;
        }
    }
}
