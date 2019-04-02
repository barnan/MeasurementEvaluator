using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;
using System.Collections.Generic;

namespace MeasurementEvaluatorUIWPF
{
    public class MainWindowParameters
    {
        [Configuration("Tab names", "Tab Names", true)]
        List<string> _tabs = null;
        public List<IPageUIWPF> Tabs { get; private set; }        // todo: fill the Title at instantiation time!!


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            Tabs = new List<IPageUIWPF>();
            foreach (string tab in _tabs)
            {
                Tabs.Add(PluginLoader.CreateInstance<IPageUIWPF>(tab));
            }

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (Tabs == null)
            {
                return false;
            }

            return true;
        }

    }
}
