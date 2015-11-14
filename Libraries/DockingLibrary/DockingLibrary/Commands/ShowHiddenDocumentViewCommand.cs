using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;

namespace DockingLibrary.Commands
{
    internal class ShowHiddenDocumentViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Debug.Assert(parameter is DocumentViewTabItemModel, "parameter must be a DocumentViewTabItemModel");

            DocumentViewTabItemModel model = parameter as DocumentViewTabItemModel;
            if (model != null)
            {
                DockingGroupTabItem tabItem = model.DocumentGroup.GetContainer(model.View);
                if (tabItem.IsOverflowHidden)
                    model.DocumentGroup.Move(model.View, 0);

                model.View.Activate();
            }
        }
    }
}
