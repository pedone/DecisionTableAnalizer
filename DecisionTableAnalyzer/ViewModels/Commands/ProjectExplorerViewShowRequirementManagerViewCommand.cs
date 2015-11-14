using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewShowRequirementManagerViewCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemRequirementManagerViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<RequirementManagerViewModel>(contextViewModel.SelectedElement.EntityId))
            {
                var viewModel = ViewModelService.Instance.QueryViewModel<RequirementManagerViewModel>(contextViewModel.SelectedElement.EntityId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
