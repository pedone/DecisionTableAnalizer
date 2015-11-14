using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewRemoveActionCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedAction != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            var selectedAction = contextViewModel.SelectedAction;

            HistoryService.Instance.BeginSession();
            
            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "DeleteSubDecisionTable";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, selectedAction.EntityId);
            ViewModelService.Instance.DeleteViewModel(selectedAction);

            HistoryService.Instance.EndSession();
        }
    }
}
