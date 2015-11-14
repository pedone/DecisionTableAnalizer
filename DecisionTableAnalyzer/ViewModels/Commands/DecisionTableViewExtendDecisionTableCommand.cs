using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class DecisionTableViewExtendDecisionTableCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            List<RuleViewModel> extendedRules = null;
            if (DecisionTableViewModelUtils.Instance.ExtendRules(contextViewModel, out extendedRules))
            {
                HistoryService.Instance.BeginSession();

                extendedRules = DecisionTableViewModelUtils.Instance.ReindexRules(extendedRules).ToList();
                contextViewModel.Rules = extendedRules;
                ViewModelService.Instance.CommitViewModel(contextViewModel);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
