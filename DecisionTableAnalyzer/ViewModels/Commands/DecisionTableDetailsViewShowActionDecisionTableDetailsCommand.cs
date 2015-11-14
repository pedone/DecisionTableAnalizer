using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    class DecisionTableDetailsViewShowActionDecisionTableDetailsCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedAction != null && contextViewModel.SelectedAction.HasReferenceSubTable;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableDetailsViewModel>(contextViewModel.SelectedAction.ReferenceSubTableId))
            {
                var viewModel = ViewModelService.Instance.QueryViewModel<DecisionTableDetailsViewModel>(contextViewModel.SelectedAction.ReferenceSubTableId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
