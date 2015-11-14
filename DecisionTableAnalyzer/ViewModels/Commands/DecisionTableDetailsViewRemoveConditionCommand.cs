using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewRemoveConditionCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            var selectedCondition = contextViewModel.SelectedCondition;

            HistoryService.Instance.BeginSession();

            string serviceId = "DTServices.DecisionTableDetailServices";
            string operationId = "DeleteSubDecisionTable";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId, selectedCondition.EntityId);
            ViewModelService.Instance.DeleteViewModel(selectedCondition);

            HistoryService.Instance.EndSession();
        }
    }
}
