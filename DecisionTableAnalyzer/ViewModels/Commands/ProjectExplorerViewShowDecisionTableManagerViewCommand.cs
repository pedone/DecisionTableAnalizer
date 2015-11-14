using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewShowDecisionTableManagerViewCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemDecisionTableManagerViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableManagerViewModel>(contextViewModel.SelectedElement.EntityId))
            {
                var viewModel = ViewModelService.Instance.QueryViewModel<DecisionTableManagerViewModel>(contextViewModel.SelectedElement.EntityId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
