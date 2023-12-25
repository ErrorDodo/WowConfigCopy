using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WowConfigCopy.UI.Extensions
{
    public class DebounceCommand<T> : ICommand where T : class
    {
        private readonly DelegateCommand<T> _innerCommand;
        private readonly int _debounceTime;
        private bool _isExecuting;

        public DebounceCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod = null, int debounceTime = 500)
        {
            _innerCommand = new DelegateCommand<T>(executeMethod, canExecuteMethod);
            _debounceTime = debounceTime;
        }

        public event EventHandler CanExecuteChanged
        {
            add { _innerCommand.CanExecuteChanged += value; }
            remove { _innerCommand.CanExecuteChanged -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _innerCommand.CanExecute((T)parameter) && !_isExecuting;
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            _isExecuting = true;
            try
            {
                _innerCommand.Execute((T)parameter);
            }
            finally
            {
                await Task.Delay(_debounceTime);
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            _innerCommand.RaiseCanExecuteChanged();
        }
    }
}