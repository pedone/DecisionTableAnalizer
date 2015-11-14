using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewRemoveActionStateCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedActionState != null && contextViewModel.SelectedAction != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();

            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "RemoveActionState";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedActionState.EntityId, contextViewModel.SelectedAction.EntityId);

            string serviceId2 = "DTServices.CommonServices";
            string operationId2 = "UpdateProjectStates";
            ViewModelService.Instance.ExecuteOperation(serviceId2, operationId2, contextViewModel.DecisionTableManagerId);

            HistoryService.Instance.EndSession();
        }
    }
}
