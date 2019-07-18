namespace MeasurementEvaluatorUIWPF.UserControls.EvaluationTabUIWPF
{
    class EvaluationTabViewModel
    {
        public EvaluationTabUIWPFParameters Parameters { get; }



        public EvaluationTabViewModel(EvaluationTabUIWPFParameters parameters)
        {
            Parameters = parameters;
            Parameters.InitializationCompleted += Parameters_InitializationCompleted;
        }

        private void Parameters_InitializationCompleted(object sender, System.EventArgs e)
        {
            Parameters.InitializationCompleted -= Parameters_InitializationCompleted;
        }

    }
}
