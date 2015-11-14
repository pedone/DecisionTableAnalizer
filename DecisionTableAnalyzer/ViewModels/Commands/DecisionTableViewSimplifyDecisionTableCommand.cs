using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class DecisionTableViewSimplifyDecisionTableCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            List<RuleViewModel> simplifiedRules;
            if (DecisionTableViewModelUtils.Instance.Simplify(contextViewModel, out simplifiedRules))
            {
                HistoryService.Instance.BeginSession();

                simplifiedRules = DecisionTableViewModelUtils.Instance.ReindexRules(simplifiedRules).ToList();
                contextViewModel.Rules = simplifiedRules;
                ViewModelService.Instance.CommitViewModel(contextViewModel);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
