using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    class DecisionTableDetailsViewShowConditionDecisionTableCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null && contextViewModel.SelectedCondition.HasReferenceSubTable;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableViewModel>(contextViewModel.SelectedCondition.ReferenceSubTableId))
            {
                var decisionTableViewModel = ViewModelService.Instance.QueryViewModel<DecisionTableViewModel>(contextViewModel.SelectedCondition.ReferenceSubTableId);
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
