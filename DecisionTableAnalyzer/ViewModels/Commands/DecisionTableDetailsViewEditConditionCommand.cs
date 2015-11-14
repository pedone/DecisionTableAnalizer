using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewEditConditionCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            var dialogModel = ViewModelService.Instance.QueryViewModel<ConditionDialogModel>(contextViewModel.SelectedCondition.EntityId);
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();
                ViewModelService.Instance.CommitViewModel(dialogModel);

                if (contextViewModel.SelectedCondition.HasReferenceSubTable)
                {
                    string serviceId = "DTServices.DecisionTableDetailServices";
                    string operationId = "SynchronizeSubDecisionTableWithReferenceElement";
                    ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedCondition.EntityId);
                }
                HistoryService.Instance.EndSession();
            }
        }
    }
}
