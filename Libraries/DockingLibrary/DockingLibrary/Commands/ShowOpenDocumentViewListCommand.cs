using System;
using System.Windows.Input;
using System.Windows.Controls;
using HelperLibrary;

namespace DockingLibrary.Commands
{
    internal class ShowOpenDocumentViewListCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            PinButton pinButton = (PinButton)parameter;
            ContextMenu documentsContextMenu = pinButton.TryFindResource("OpenDocumentsContextMenu") as ContextMenu;
            if (documentsContextMenu == null)
                return;

            if (documentsContextMenu.DataContext == null)
            {
                documentsContextMenu.DataContext = TreeHelper.FindVisualAncestor<DocumentGroup>(pinButton);
                documentsContextMenu.PlacementTarget = pinButton;

                documentsContextMenu.Opened += (sender, e) => pinButton.KeepButtonPressed = true;
                documentsContextMenu.Closed += (sender, e) => pinButton.KeepButtonPressed = false;
            }
            else
                documentsContextMenu.GetBindingExpression(ContextMenu.ItemsSourceProperty).UpdateTarget();

            documentsContextMenu.IsOpen = true;
        }

    }
}
