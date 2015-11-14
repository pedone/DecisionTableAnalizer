using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewConvertActionToFromDecisionTableCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedAction != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();
            if (contextViewModel.SelectedAction.HasReferenceSubTable == false)
            {
                string serviceId = "DTServices.DecisionTableDetailServices";
                string operationId = "CreateSubDecisionTable";
                ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedAction.EntityId);
            }
            else
            {
                string serviceId = "DTServices.DecisionTableDetailServices";
                string operationId = "DeleteSubDecisionTable";
                ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedAction.EntityId);
            }
            HistoryService.Instance.EndSession();
        }
    }
}
