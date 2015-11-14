using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewRemoveConditionStateCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedConditionState != null && contextViewModel.SelectedCondition != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();

            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "RemoveConditionState";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedConditionState.EntityId, contextViewModel.SelectedCondition.EntityId);

            string serviceId2 = "DTServices.CommonServices";
            string operationId2 = "UpdateProjectStates";
            ViewModelService.Instance.ExecuteOperation(serviceId2, operationId2, contextViewModel.DecisionTableManagerId);

            HistoryService.Instance.EndSession();
        }
    }
}
