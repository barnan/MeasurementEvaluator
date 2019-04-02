using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF;
using MeasurementEvaluatorUIWPF.UserControls.EvaluationTab;
using System;
using System.Collections.Generic;

namespace MeasurementEvaluatorUIWPF
{
    public class Factory : IPluginFactory
    {

        IMainWindowUIWPF _mainWindow;
        IPageUIWPF _mainPage;
        private Dictionary<string, IPageUIWPF> _pages;



        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(MainWindow)))
            {
                if (_mainWindow == null)
                {
                    MainWindowParameters param = new MainWindowParameters();
                    if (param.Load(name))
                    {
                        MainWindow instance = new MainWindow(param);
                        _mainWindow = instance;
                    }
                }
                return _mainWindow;
            }

            if (t.IsAssignableFrom(typeof(IUserControlUIWPF)))
            {
                if (name.Contains("EvaluationTab"))
                {
                    EvaluationTabUIWPFParameters param = new EvaluationTabUIWPFParameters();
                    if (param.Load(name))
                    {
                        EvaluationTabUIWPF instance = new EvaluationTabUIWPF(param);
                        return instance;
                    }
                }


                if (name.Contains("DataCollector"))
                {
                    DataCollectorUIWPFParameters param = new DataCollectorUIWPFParameters();
                    if (param.Load(name))
                    {
                        DataCollectorUIWPF instance = new DataCollectorUIWPF(param);
                        return instance;
                    }
                }

                if (name.Contains("ResultHandling"))
                {
                    return null;
                }

                if (name.Contains("DataCollector"))
                {
                    return null;
                }

            }


            return null;
        }
    }
}
