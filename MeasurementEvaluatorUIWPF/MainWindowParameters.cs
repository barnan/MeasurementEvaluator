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
        public List<ITabUIWPF> Tabs { get; private set; }        // todo: fill the Title at instantiation time!!


        public string ID { get; private set; }


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            ID = sectionName;

            Tabs = new List<ITabUIWPF>();
            foreach (string tab in _tabs)
            {
                Tabs.Add(PluginLoader.CreateInstance<ITabUIWPF>(tab));
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
