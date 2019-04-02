using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using MeasurementEvaluatorUIWPF.Pages.EditorPageUIWPF;
using MeasurementEvaluatorUIWPF.Pages.EvaluationPage;
using MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF;
using MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF;
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

            if (t.IsAssignableFrom(typeof(IPageUIWPF)))
            {
                if (name.Contains("MainPage"))
                {
                    if (_mainPage == null)
                    {
                        MainPageUIWPFParameters param = new MainPageUIWPFParameters();
                        if (param.Load(name))
                        {
                            MainPageUIWPF instance = new MainPageUIWPF(param);
                            _mainPage = instance;
                        }
                    }
                    return _mainPage;
                }

                if (name.Contains("Evaluation"))
                {
                    EvaluationPageUIWPFParameters param = new EvaluationPageUIWPFParameters();
                    if (param.Load(name))
                    {
                        EvaluationPageUIWPF instance = new EvaluationPageUIWPF(param);
                        return instance;
                    }
                }

                if (name.Contains("Editor"))
                {
                    EditorPageUIWPFParameters param = new EditorPageUIWPFParameters();
                    if (param.Load(name))
                    {
                        EditorPageUIWPF instance = new EditorPageUIWPF(param);
                        return instance;
                    }
                }
            }


            if (t.IsAssignableFrom(typeof(IUserControlUIWPF)))
            {
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
