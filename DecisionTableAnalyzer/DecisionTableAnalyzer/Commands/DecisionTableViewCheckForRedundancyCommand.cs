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
    public class DecisionTableViewCheckForRedundancyCommand : DTCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DecisionTable != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            var redundantRules = contextViewModel.DecisionTable.CheckForRedundancy();
            if (redundantRules.Count() > 0)
            {
                RedundantRulesDialogModel dialogModel = new RedundantRulesDialogModel { RedundantRules = redundantRules };
                RedundantRulesDialog dialog = new RedundantRulesDialog { DataContext = dialogModel, Owner = App.Current.MainWindow };
                if (dialog.ShowDialog() == true)
                    contextViewModel.DecisionTable.RemoveRedundantRules();
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "There are no redundant rules.", "Decision Table Analyzer");
            }
        }
    }
}
