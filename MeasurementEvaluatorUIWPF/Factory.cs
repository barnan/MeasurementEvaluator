using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF;
using MeasurementEvaluatorUIWPF.UserControls.EditorTabUIWPF;
using MeasurementEvaluatorUIWPF.UserControls.EvaluationTabUIWPF;
using System;

namespace MeasurementEvaluatorUIWPF
{
    public class Factory : IPluginFactory
    {

        IWindowUIWPF _window;


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(Window)))
            {
                if (_window == null)
                {
                    MainWindowParameters param = new MainWindowParameters();
                    if (param.Load(name))
                    {
                        Window instance = new Window(param);
                        _window = instance;
                    }
                }
                return _window;
            }

            if (t.IsAssignableFrom(typeof(ITabUIWPF)))
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

                if (name.Contains("EditorTab"))
                {
                    EditorTabUIWPFParameters param = new EditorTabUIWPFParameters();
                    if (param.Load(name))
                    {
                        EditorTabUIWPF instance = new EditorTabUIWPF(param);
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
