using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewMoveConditionBottomCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null &&
                contextViewModel.Conditions.IndexOf(contextViewModel.SelectedCondition) < contextViewModel.Conditions.Count - 1;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();

            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "MoveConditionBottom";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedCondition.EntityId);

            HistoryService.Instance.EndSession();
        }
    }
}
