using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewShowDecisionTableDataCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemDecisionTableViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableDetailsViewModel>(contextViewModel.SelectedElement.EntityId))
            {
                var viewModel = ViewModelService.Instance.QueryViewModel<DecisionTableDetailsViewModel>(contextViewModel.SelectedElement.EntityId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
