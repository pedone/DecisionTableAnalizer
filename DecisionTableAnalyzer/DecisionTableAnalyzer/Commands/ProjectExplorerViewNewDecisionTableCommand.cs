using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewNewDecisionTableCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            DecisionTableDialogModel dialogModel = new DecisionTableDialogModel();
            DecisionTableDialog dialog = new DecisionTableDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow
            };
            if (dialog.ShowDialog() == true)
            {
                DecisionTable newDecisionTable = new DecisionTable
                {
                    Name = dialogModel.Name,
                    Description = dialogModel.Description
                };

                if (contextViewModel.Project.DecisionTableManager.DecisionTables.Any(cur => cur.Name == newDecisionTable.Name))
                    MessageBox.Show("A decision table with that name already exists.", "Error");
                else
                    contextViewModel.Project.DecisionTableManager.Add(newDecisionTable);
            }
        }
    }
}
