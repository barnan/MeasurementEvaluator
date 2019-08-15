using MeasurementEvaluatorUI.Base;
using System;
using System.Windows.Input;

namespace MeasurementEvaluatorUI.Commands
{
    internal class RelayCommand : NotifyPropertyChanged, ICommand
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

                var eventHandlerToCall = CanExecuteChanged;
                eventHandlerToCall?.Invoke(this, new EventArgs());
            }
        }

        public void UpdateCanExecute()
        {
            EventHandler handler = CanExecuteChanged;
            handler?.Invoke(this, new EventArgs());
        }

        #endregion

        #region ICommand

        public event EventHandler CanExecuteChanged;
        //{
        //    add { CommandManager.RequerySuggested += value; }       // todo: look after -> what is CommandManager
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

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

    }
}
