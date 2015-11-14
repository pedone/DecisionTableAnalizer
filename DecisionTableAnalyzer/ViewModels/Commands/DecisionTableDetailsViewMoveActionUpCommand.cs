using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewMoveActionUpCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedAction != null &&
                contextViewModel.Actions.IndexOf(contextViewModel.SelectedAction) > 0;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();

            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "MoveActionUp";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedAction.EntityId);

            HistoryService.Instance.EndSession();
        }
    }
}
