using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    class DecisionTableDetailsViewShowActionDecisionTableCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedAction != null && contextViewModel.SelectedAction.HasReferenceSubTable;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableViewModel>(contextViewModel.SelectedAction.ReferenceSubTableId))
            {
                var decisionTableViewModel = ViewModelService.Instance.QueryViewModel<DecisionTableViewModel>(contextViewModel.SelectedAction.ReferenceSubTableId);

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
