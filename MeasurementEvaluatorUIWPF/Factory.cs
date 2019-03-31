using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using MeasurementEvaluatorUIWPF.Pages.EvaluationPage;
using MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF;
using System;

namespace MeasurementEvaluatorUIWPF
{
    public class Factory : IPluginFactory
    {

        IMainWindowUIWPF _mainWindow;
        IPageUIWPF _mainPage;


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
                    return _mainWindow;
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
            }

            return null;
        }
    }
}
