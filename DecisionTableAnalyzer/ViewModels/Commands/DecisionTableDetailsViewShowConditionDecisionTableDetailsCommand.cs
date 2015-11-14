using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    class DecisionTableDetailsViewShowConditionDecisionTableDetailsCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null && contextViewModel.SelectedCondition.HasReferenceSubTable;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableDetailsViewModel>(contextViewModel.SelectedCondition.ReferenceSubTableId))
            {
                var viewModel = ViewModelService.Instance.QueryViewModel<DecisionTableDetailsViewModel>(contextViewModel.SelectedCondition.ReferenceSubTableId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
