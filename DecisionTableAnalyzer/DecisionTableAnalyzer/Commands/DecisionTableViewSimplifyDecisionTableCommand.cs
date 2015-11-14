using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableViewSimplifyDecisionTableCommand : DTCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DecisionTable != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            //contextViewModel.ResetTableView();

            contextViewModel.DecisionTable.SimplifyTable();
            //contextViewModel.RefreshTableViewCommand.Execute(contextViewModel);
        }
    }
}
