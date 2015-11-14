using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using System.Windows;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableManagerViewNewDecisionTableCommand : DTCommand<DecisionTableManagerViewModel>
    {

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            DecisionTableDialogModel dialogModel = new DecisionTableDialogModel();
            DecisionTableDialog dialog = new DecisionTableDialog
            {
                DataContext = dialogModel,
                Owner = Application.Current.MainWindow
            };
            if (dialog.ShowDialog() == true)
            {
                DecisionTable newDecisionTable = new DecisionTable
                {
                    Name = dialogModel.Name,
                    Description = dialogModel.Description
                };

                if (contextViewModel.DecisionTableManager.DecisionTables.Any(cur => cur.Name == newDecisionTable.Name))
                    MessageBox.Show("A decision table with that name already exists.", "Error");
                else
                    contextViewModel.DecisionTableManager.Add(newDecisionTable);
            }
        }

        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return true;
        }

    }
}
