using System;
using System.Windows.Input;

namespace Editor
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute((T)parameter);
        }

        public RelayCommand(Action<T> execute, Predicate<T>? canExecute)
        {
            _execute = execute ?? throw new Exception("Added event is non existent");
            _canExecute = canExecute;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public RelayCommand(Action execute)
        {
            _execute = execute ?? throw new Exception("Added event is non existent");
        }
    }
}
