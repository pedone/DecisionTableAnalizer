using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewNewRequirementCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            RequirementDialogModel dialogModel = new RequirementDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                string serviceId = "DTServices.CommonServices";
                string operationId = "GetRequirementManager";
                var requirementManagerViewModel = ViewModelService.Instance.ExecuteOperation<RequirementManagerViewModel>(serviceId, operationId, contextViewModel.Project.EntityId);

                HistoryService.Instance.BeginSession();

                dialogModel.RequirementManagerId = requirementManagerViewModel.EntityId;
                string ownerCollection = dialogModel.Kind == DTEnums.RequirementKind.Functional ? "FunctionalRequirements" : "NonFunctionalRequirements";
                ViewModelService.Instance.InsertViewModel(dialogModel, requirementManagerViewModel.EntityId, ownerCollection);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
