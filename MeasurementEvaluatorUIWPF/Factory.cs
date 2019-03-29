using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using System;

namespace MeasurementEvaluatorUIWPF
{
    public class Factory : IPluginFactory
    {

        IMainWindowUIWPF _mainWindow;


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
            return null;
        }
    }
}
