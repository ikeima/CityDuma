using System;
using System.Windows.Input;

namespace CityDuma.ViewModels
{
    public class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Func<bool> canExecute;

        public RelayCommand(Action<T> execute, Func<bool> canExecute = null)
        {
            this._execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
    internal class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this._execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}