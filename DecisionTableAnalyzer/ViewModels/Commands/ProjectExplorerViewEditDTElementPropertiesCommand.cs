using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewEditDTElementPropertiesCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null &&
                (contextViewModel.SelectedElement is SystemConditionViewModel || contextViewModel.SelectedElement is SystemActionViewModel);
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            ViewModel dialogModel = null;
            if (contextViewModel.SelectedElement is SystemConditionViewModel)
                dialogModel = ViewModelService.Instance.QueryViewModel<ConditionDialogModel>(contextViewModel.SelectedElement.EntityId);
            else if (contextViewModel.SelectedElement is SystemActionViewModel)
                dialogModel = ViewModelService.Instance.QueryViewModel<ActionDialogModel>(contextViewModel.SelectedElement.EntityId);
            else
                return;

            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();

                ViewModelService.Instance.CommitViewModel(dialogModel);

                string serviceId = "DTServices.DecisionTableDetailServices";
                string operationId = "SynchronizeSubDecisionTableWithReferenceElement";
                ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedElement.EntityId);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
