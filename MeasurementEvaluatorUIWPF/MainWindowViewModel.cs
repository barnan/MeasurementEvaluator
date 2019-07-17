using Interfaces.Misc;
using MeasurementEvaluatorUI.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

            Parameters.Closed += Parameters_OnClosed;

            Tabs = new ObservableCollection<TabItem>();

            foreach (ITabUIWPF tabUiwpf in parameters.Tabs)
            {
                Tabs.Add(new TabItem { Title = tabUiwpf.Title, Content = tabUiwpf });
            }

            SelectedTabItem = Tabs[0];
        }

        private void Parameters_OnClosed(object sender, EventArgs eventArgs)
        {


        }
    }
}
