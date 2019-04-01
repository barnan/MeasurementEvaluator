using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;

namespace MeasurementEvaluatorUIWPF
{
    internal class MainWindowParameters
    {
        [Configuration("Name of the MainPage", "MainPage", true)]
        private string _mainPage = null;
        internal IPageUIWPF MainPage { get; private set; }


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

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
