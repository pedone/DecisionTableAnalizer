using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DecisionTableAnalyzer.ViewModels;

namespace DecisionTableAnalyzer.Commands
{
    public sealed class DelegateCommand<T> : ICommand
    {

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DTCommand<T> CommandInstance { get; set; }

        public DelegateCommand(DTCommand<T> commandInstance)
        {
            CommandInstance = commandInstance;
        }

        bool CanExecute(object parameter)
        {
            return CommandInstance.CanExecute((T)parameter);
        }

        void Execute(object parameter)
        {
            CommandInstance.Execute((T)parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }
    }
}
