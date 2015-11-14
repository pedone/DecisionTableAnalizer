using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewDeleteRequirementCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemRequirementViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();
            ViewModelService.Instance.DeleteViewModel(contextViewModel.SelectedElement);
            HistoryService.Instance.EndSession();
        }
    }
}
