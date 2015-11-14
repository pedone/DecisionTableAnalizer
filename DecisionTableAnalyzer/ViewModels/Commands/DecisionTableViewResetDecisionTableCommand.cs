using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class DecisionTableViewResetDecisionTableCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();

            var rules = DecisionTableViewModelUtils.Instance.BuildRules(contextViewModel);
            contextViewModel.Rules = rules;
            ViewModelService.Instance.CommitViewModel(contextViewModel);

            HistoryService.Instance.EndSession();
        }

    }
}
