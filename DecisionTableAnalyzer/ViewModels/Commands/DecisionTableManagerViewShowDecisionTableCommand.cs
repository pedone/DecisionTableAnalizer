using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    class DecisionTableManagerViewShowDecisionTableCommand : ViewModelCommand<DecisionTableManagerViewModel>
    {
        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedDecisionTable != null;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableViewModel>(contextViewModel.SelectedDecisionTable.EntityId))
            {
                var decisionTableViewModel = ViewModelService.Instance.QueryViewModel<DecisionTableViewModel>(contextViewModel.SelectedDecisionTable.EntityId);

                if (decisionTableViewModel.Rules.Count == 0)
                {
                    var rules = DecisionTableViewModelUtils.Instance.BuildRules(decisionTableViewModel);
                    if (rules.Count > 0)
                    {
                        string ownerCollection = "Rules";
                        ViewModelService.Instance.InsertViewModels(rules, decisionTableViewModel.EntityId, ownerCollection);
                    }
                }

                ViewService.Instance.ShowView(decisionTableViewModel);
            }
        }

    }
}
