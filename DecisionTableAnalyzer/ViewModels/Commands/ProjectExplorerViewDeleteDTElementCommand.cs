using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewDeleteDTElementCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null &&
                (contextViewModel.SelectedElement is SystemConditionViewModel || contextViewModel.SelectedElement is SystemActionViewModel);
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var selectedElement = contextViewModel.SelectedElement;

            HistoryService.Instance.BeginSession();

            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "DeleteSubDecisionTable";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, selectedElement.EntityId);
            ViewModelService.Instance.DeleteViewModel(selectedElement);

            HistoryService.Instance.EndSession();
        }
    }
}
