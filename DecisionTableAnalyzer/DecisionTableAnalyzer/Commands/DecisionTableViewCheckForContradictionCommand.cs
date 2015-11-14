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
    public class DecisionTableViewCheckForContradictionCommand : DTCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DecisionTable != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            var contradictedRules = contextViewModel.DecisionTable.CheckForContradiction();
            if (contradictedRules.Count() > 0)
            {
                ContradictedRulesDialogModel dialogModel = new ContradictedRulesDialogModel(contradictedRules);
                ContradictedRulesDialog dialog = new ContradictedRulesDialog { Owner = App.Current.MainWindow, DataContext = dialogModel };
                if (dialog.ShowDialog() == true)
                {
                    foreach (var rule in dialogModel.RulesToRemove.OrderByDescending(cur => cur.Index))
                        contextViewModel.DecisionTable.RemoveRule(rule.Index);
                }
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "There are no contradictions.", "Decision Table Analyzer", MessageBoxButton.OK);
            }
        }
    }
}
