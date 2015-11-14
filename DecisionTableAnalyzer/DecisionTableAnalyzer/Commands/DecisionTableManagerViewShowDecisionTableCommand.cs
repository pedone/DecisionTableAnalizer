using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    class DecisionTableManagerViewShowDecisionTableCommand : DTCommand<DecisionTableManagerViewModel>
    {
        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return contextViewModel != null &&
                contextViewModel.DockManager != null &&
                contextViewModel.SelectedDecisionTable != null;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            DecisionTableView view = contextViewModel.DockManager.ShowView<DecisionTableView>(activate: true);
            view.DataContext = new DecisionTableViewModel(contextViewModel.SelectedDecisionTable);
        }
    }
}
