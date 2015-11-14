using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewMoveConditionUpCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null &&
                contextViewModel.Conditions.IndexOf(contextViewModel.SelectedCondition) > 0;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();

            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "MoveConditionUp";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedCondition.EntityId);

            HistoryService.Instance.EndSession();
        }
    }
}
