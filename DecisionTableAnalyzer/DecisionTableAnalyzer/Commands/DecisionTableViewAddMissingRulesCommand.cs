using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableViewAddMissingRulesCommand : DTCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DecisionTable != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            contextViewModel.DecisionTable.AddMissingRules();
        }

    }
}
