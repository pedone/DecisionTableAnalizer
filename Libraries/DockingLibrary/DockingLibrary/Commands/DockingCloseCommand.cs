using System;
using System.Windows.Input;
using System.Diagnostics;

namespace DockingLibrary.Commands
{
    internal class DockingCloseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Debug.Assert(parameter is View, "Parameter must be a view.");

            View view = parameter as View;
            if (view != null)
                view.Hide();
        }

    }
}
