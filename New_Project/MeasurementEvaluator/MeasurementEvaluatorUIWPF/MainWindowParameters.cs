using System.Windows.Threading;
using Frame.Configuration;
using Frame.PluginLoader;
using Interfaces.IUIWPF;
using MeasurementEvaluatorUIWPF.Base;

namespace MeasurementEvaluatorUIWPF
{
    internal class MainWindowParameters : ParameterBase
    {
        [Configuration("Tab names", "Tab Names", true)]
        List<string> _tabs = null;
        public List<ITabUIWPF> Tabs { get; private set; }        // todo: fill the Title at instantiation time!!


        public MessageControlUIWPF.MessageControlUIWPF MessageControlUIWPF { get; private set; }


        public MainWindowParameters()
        {
            MainWindowDispatcher = Dispatcher.CurrentDispatcher;
        }



        internal bool Load(string sectionName)
        {
            MessageControlUIWPF = PluginLoader.CreateInstance<MessageControlUIWPF.MessageControlUIWPF>("MessageControl");

            PluginLoader.ConfigManager.Load(this, sectionName);

            Name = sectionName;

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
