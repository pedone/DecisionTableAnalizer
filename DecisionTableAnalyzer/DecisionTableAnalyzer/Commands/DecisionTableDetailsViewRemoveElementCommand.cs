using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableDetailsViewRemoveElementCommand : DTCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            contextViewModel.DecisionTable.Remove(contextViewModel.SelectedElement);
        }
    }
}
