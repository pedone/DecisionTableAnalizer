using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewConvertConditionToFromDecisionTableCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();
            if (contextViewModel.SelectedCondition.HasReferenceSubTable == false)
            {
                string serviceId = "DTServices.DecisionTableDetailServices";
                string operationId = "CreateSubDecisionTable";
                ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedCondition.EntityId);
            }
            else
            {
                string serviceId = "DTServices.DecisionTableDetailServices";
                string operationId = "DeleteSubDecisionTable";
                ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedCondition.EntityId);
            }
            HistoryService.Instance.EndSession();
        }
    }
}
