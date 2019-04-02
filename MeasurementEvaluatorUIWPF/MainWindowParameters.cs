using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;

namespace MeasurementEvaluatorUIWPF
{
    public class MainWindowParameters
    {
        [Configuration("Name of the MainPage", "MainPage Name", true)]
        private string _mainPage = null;
        public IPageUIWPF MainPage { get; private set; }


        internal bool Load(string sectionName)
        {
            if (!PluginLoader.ConfigManager.Load(this, sectionName))
            {
                return false;
            }

            MainPage = PluginLoader.CreateInstance<IPageUIWPF>(_mainPage);

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (MainPage == null)
            {
                return false;
            }

            return true;
        }

    }
}
