using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MeasurementEvaluatorUI.Commands
{
    class RelayCommand : ICommand, INotifyPropertyChanged
    {
        #region fields

        private readonly Action<object> _parameterizedAction;
        private readonly Action _simpleAction;
        private readonly Predicate<object> _canExecute;

        #endregion

        #region ctor

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _parameterizedAction = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            _simpleAction = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region properties

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ICommand

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }       // ??????
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled && (_canExecute == null || _canExecute(parameter));
        }

        public void Execute(object parameter)
        {
            _simpleAction?.Invoke();

            _parameterizedAction?.Invoke(parameter);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
