using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DTCore;

namespace DTCore
{
    internal sealed class DelegateCommand : ICommand
    {

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ViewModelCommand CommandInstance { get; set; }

        public DelegateCommand(ViewModelCommand commandInstance)
        {
            CommandInstance = commandInstance;
        }

        bool CanExecute(object parameter)
        {
            return CommandInstance.CanExecute((ViewModel)parameter);
        }

        void Execute(object parameter)
        {
            CommandInstance.Execute((ViewModel)parameter);
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
