using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    class DecisionTableManagerViewShowDecisionTableDetailsCommand : ViewModelCommand<DecisionTableManagerViewModel>
    {
        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedDecisionTable != null;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableDetailsViewModel>(contextViewModel.SelectedDecisionTable.EntityId))
            {
                var viewModel = ViewModelService.Instance.QueryViewModel<DecisionTableDetailsViewModel>(contextViewModel.SelectedDecisionTable.EntityId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
