using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewNewActionCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null && contextViewModel.SelectedItemModel is DecisionTable;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            DTElementDialogModel dialogModel = new DTElementDialogModel { Kind = DTElementKind.Condition };
            DTElementDialog dialog = new DTElementDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow
            };
            if (dialog.ShowDialog() == true)
            {
                var decisionTable = contextViewModel.SelectedItemModel as DecisionTable;
                DTElement newElement = new DTElement
                {
                    Name = dialogModel.Name,
                    Kind = DTElementKind.Action,
                    Description = dialogModel.Description
                };

                if (decisionTable.Actions.Any(cur => cur.Name == newElement.Name))
                    MessageBox.Show("An action with that name already exists.", "Error");
                else
                    decisionTable.Add(newElement);
            }
        }
    }
}
