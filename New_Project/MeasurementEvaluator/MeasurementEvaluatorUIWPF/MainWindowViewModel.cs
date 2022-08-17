using System.Collections.ObjectModel;
using System.Windows.Controls;
using Interfaces.IUIWPF;
using MeasurementEvaluatorUIWPF.Base;

namespace MeasurementEvaluatorUIWPF
{
    internal class MainWindowViewModel : ViewModelBase
    {

        public MainWindowParameters Parameters { get; }


        public IList<TabItem> Tabs { get; }


        private TabItem _selectedTabItem;
        public TabItem SelectedTabItem
        {
            get => _selectedTabItem;
            set
            {
                _selectedTabItem = value;
                OnPropertyChanged();
            }
        }


        internal MainWindowViewModel(MainWindowParameters parameters)
        {
            Parameters = parameters;

            Tabs = new ObservableCollection<TabItem>();

            foreach (ITabUIWPF tabUiwpf in parameters.Tabs)
            {
                Tabs.Add(new TabItem { Name = tabUiwpf.Title, Content = tabUiwpf });
            }

            SelectedTabItem = Tabs[0];
        }

    }
}
