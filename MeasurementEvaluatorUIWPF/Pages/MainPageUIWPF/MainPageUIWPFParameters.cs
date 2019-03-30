using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;
using System.Collections.Generic;

namespace MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF
{
    internal class MainPageUIWPFParameters
    {
        [Configuration("Tabpage names", nameof(TabPages), true)]
        List<string> _tabPages = null;
        internal List<IMainPageUIWPF> TabPages { get; private set; }        // todo: fill the Title at instantiation time!!


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            TabPages = new List<IMainPageUIWPF>();
            foreach (string tabpage in _tabPages)
            {
                TabPages.Add(PluginLoader.CreateInstance<IMainPageUIWPF>(tabpage));
            }

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (TabPages == null)
            {
                return false;
            }

            return true;
        }
    }
}