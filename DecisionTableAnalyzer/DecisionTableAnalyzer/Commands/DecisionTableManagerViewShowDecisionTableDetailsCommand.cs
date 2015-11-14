using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    class DecisionTableManagerViewShowDecisionTableDetailsCommand : DTCommand<DecisionTableManagerViewModel>
    {
        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return contextViewModel != null &&
                contextViewModel.DockManager != null &&
                contextViewModel.SelectedDecisionTable != null;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            DecisionTableDetailsView dtDataView = contextViewModel.DockManager.ShowView<DecisionTableDetailsView>(activate: true);
            dtDataView.DataContext = new DecisionTableDetailsViewModel(contextViewModel.SelectedDecisionTable);
        }
    }
}
