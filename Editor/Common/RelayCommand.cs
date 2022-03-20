using System;
using System.Windows.Input;

namespace Editor
{
    //Implementing the command interface
    public class RelayCommand<T> : ICommand
    {
        //The execute and can execute functions
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute;

        //A alowing for automatic rechecking of can execute
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //Checks if the command can be executed
        //A null predicate is assumed to be always active
        public bool CanExecute(object? parameter)
        {
            //If the parameter is null,
            //we will assume no check is needed
            if (parameter == null)
                return true;

            //Invoke the can execute action
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object? parameter)
        {
            //If there is no parameter, do nothing
            if (parameter == null)
                return;

            //Invoke execute
            _execute((T)parameter);
        }

        //The constructor takes a non nullable execute action and a nullable
        //can execute predicate
        public RelayCommand(Action<T> execute, Predicate<T>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //The constructor takes a non nullable execute action only
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        { }
    }

    //A relay command with no type arguments and no predicate
    public class RelayCommand : ICommand
    {
        //The action to execute
        private readonly Action _execute;

        //A alowing for automatic rechecking of can execute
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //This is always true
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        //Execute the action
        public void Execute(object? parameter)
        {
            _execute();
        }

        //The constructor takes one non nullable action
        public RelayCommand(Action execute)
        {
            _execute = execute;
        }
    }
}
