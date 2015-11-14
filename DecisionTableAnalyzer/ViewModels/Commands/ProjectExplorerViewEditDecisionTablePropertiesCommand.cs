using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewEditDecisionTablePropertiesCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemDecisionTableViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var dialogModel = ViewModelService.Instance.QueryViewModel<DecisionTableDialogModel>(contextViewModel.SelectedElement.EntityId);
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();

                ViewModelService.Instance.CommitViewModel(dialogModel);

                string serviceId = "DTServices.DecisionTableDetailServices";
                string operationId = "SynchronizeReferenceElementWithSubDecisionTable";
                ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedElement.EntityId);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
