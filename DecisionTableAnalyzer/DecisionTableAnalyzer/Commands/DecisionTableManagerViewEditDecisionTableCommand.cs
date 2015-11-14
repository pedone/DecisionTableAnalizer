using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableManagerViewEditDecisionTableCommand : DTCommand<DecisionTableManagerViewModel>
    {
        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedDecisionTable != null;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            DecisionTableDialogModel dialogModel = new DecisionTableDialogModel
            {
                Name = contextViewModel.SelectedDecisionTable.Name,
                Description = contextViewModel.SelectedDecisionTable.Description,
            };
            DecisionTableDialog dialog = new DecisionTableDialog
            {
                DataContext = dialogModel,
                Owner = Application.Current.MainWindow,
            };

            if (dialog.ShowDialog() == true)
            {
                if (contextViewModel.DecisionTableManager.DecisionTables.Except(new[] { contextViewModel.SelectedDecisionTable }).Any(cur => cur.Name == dialogModel.Name))
                {
                    MessageBox.Show("A decision table with that name already exists.", "Error");
                }
                else
                {
                    contextViewModel.SelectedDecisionTable.Name = dialogModel.Name;
                    contextViewModel.SelectedDecisionTable.Description = dialogModel.Description;
                }
            }
        }
    }
}
