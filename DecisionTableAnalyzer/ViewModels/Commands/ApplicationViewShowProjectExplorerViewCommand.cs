using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewShowProjectExplorerViewCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<ProjectExplorerViewModel>())
            {
                var viewModel = ViewModelService.Instance.QueryViewModel<ProjectExplorerViewModel>(contextViewModel.CurrentProject.EntityId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
