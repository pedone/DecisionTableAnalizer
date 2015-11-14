using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewEditProjectPropertiesCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            ProjectDialogModel dialogModel = ViewModelService.Instance.QueryViewModel<ProjectDialogModel>(contextViewModel.Project.EntityId);
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();
                ViewModelService.Instance.CommitViewModel(dialogModel);
                HistoryService.Instance.EndSession();
            }
        }
    }
}
