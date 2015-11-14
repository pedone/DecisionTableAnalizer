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
    public class DecisionTableViewCheckForCompletenessCommand : DTCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DecisionTable != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            var missingRules = contextViewModel.DecisionTable.CheckForCompleteness();
            if (missingRules.Count() > 0)
            {
                string missingRuleCounterString = missingRules.Count() > 1 ? "are" : "is";
                if (MessageBox.Show(App.Current.MainWindow,
                    string.Format("There {0} {1} missing rule(s).\nDo You want to add all missing rules?", missingRuleCounterString, missingRules.Count()),
                    "Decision Table Analyzer",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    contextViewModel.DecisionTable.AddMissingRules();
                }
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "All Conditions have been covered.", "Decision Table Analyzer", MessageBoxButton.OK);
            }
        }
    }
}
