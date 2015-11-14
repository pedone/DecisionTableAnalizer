using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using System.Windows;

namespace ViewModels.Commands
{
    public class ApplicationViewNewProjectCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            ProjectDialogModel dialogModel = new ProjectDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                string serviceId = "DTServices.ProjectServices";
                string operationId = "CreateProject";
                ViewModelService.Instance.Unload();
                var projectId = ViewModelService.Instance.ExecuteOperation<EntityId>(serviceId, operationId, dialogModel);
                contextViewModel.InitProject(projectId);
            }
        }
    }
}
