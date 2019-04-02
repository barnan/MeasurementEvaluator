﻿using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;
using System.Collections.Generic;

namespace MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF
{
    public class MainPageUIWPFParameters
    {
        [Configuration("Tabpage names", "TabPage Names", true)]
        List<string> _tabPages = null;
        public List<IPageUIWPF> TabPages { get; private set; }        // todo: fill the Title at instantiation time!!


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            TabPages = new List<IPageUIWPF>();
            foreach (string tabpage in _tabPages)
            {
                TabPages.Add(PluginLoader.CreateInstance<IPageUIWPF>(tabpage));
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