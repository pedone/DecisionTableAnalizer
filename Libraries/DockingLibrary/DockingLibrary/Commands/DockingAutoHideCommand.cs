using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace DockingLibrary.Commands
{
    internal class DockingAutoHideCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Debug.Assert(parameter is ViewGroup, "Parameter must be a ViewGroup.");

            ViewGroup viewGroup = parameter as ViewGroup;
            if (viewGroup != null && viewGroup.DockState != DockState.AutoHide)
            {
                DockManager dockManager = viewGroup.DockManager;
                dockManager.ResetActiveViewToCenterContent(true);

                dockManager.LockActiveView();
                viewGroup.SetDockState(DockState.AutoHide);
                dockManager.UnlockActiveView();
            }
        }

    }
}
