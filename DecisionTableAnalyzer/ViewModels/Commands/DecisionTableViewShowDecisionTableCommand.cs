using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels.Utils;
using System.Windows.Input;

namespace ViewModels.Commands
{
    class DecisionTableViewShowDecisionTableCommand : ViewModelCommand<ViewModel>
    {
        public override bool CanExecute(ViewModel contextViewModel)
        {
            return (contextViewModel is ConditionViewModel && ((ConditionViewModel)contextViewModel).HasReferenceSubTable) ||
                (contextViewModel is ActionViewModel && ((ActionViewModel)contextViewModel).HasReferenceSubTable);
        }

        public override void Execute(ViewModel contextViewModel)
        {
            EntityId subTableId = null;
            if (contextViewModel is ConditionViewModel)
                subTableId = ((ConditionViewModel)contextViewModel).ReferenceSubTableId;
            else if (contextViewModel is ActionViewModel)
                subTableId = ((ActionViewModel)contextViewModel).ReferenceSubTableId;

            if (subTableId == null)
                return;

            if (!ViewService.Instance.ShowViewExistingView<DecisionTableViewModel>(subTableId))
            {
                var decisionTableViewModel = ViewModelService.Instance.QueryViewModel<DecisionTableViewModel>(subTableId);

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
