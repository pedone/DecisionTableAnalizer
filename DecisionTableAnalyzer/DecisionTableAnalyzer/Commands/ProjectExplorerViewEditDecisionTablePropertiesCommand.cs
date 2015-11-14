using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewEditDecisionTablePropertiesCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedItemModel is DecisionTable;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var selectedTable = contextViewModel.SelectedItemModel as DecisionTable;

            DecisionTableDialogModel dialogModel = new DecisionTableDialogModel
            {
                Name = selectedTable.Name,
                Description = selectedTable.Description,
            };
            DecisionTableDialog dialog = new DecisionTableDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow,
            };

            if (dialog.ShowDialog() == true)
            {
                if (contextViewModel.Project.DecisionTableManager.DecisionTables.Except(new[] { selectedTable }).Any(cur => cur.Name == dialog.Name))
                {
                    MessageBox.Show("A decision table with that name already exists.", "Error");
                }
                else
                {
                    selectedTable.Name = dialogModel.Name;
                    selectedTable.Description = dialogModel.Description;
                }
            }
        }
    }
}
