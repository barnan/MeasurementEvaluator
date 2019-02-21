using MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF;

namespace MeasurementEvaluatorUIWPF
{
    public class MainWindowParameters
    {
        MainPageUIWPF MainPage { get; }


        public MainWindowParameters(MainPageUIWPF mainPage)
        {
            MainPage = mainPage;
        }

    }
}
