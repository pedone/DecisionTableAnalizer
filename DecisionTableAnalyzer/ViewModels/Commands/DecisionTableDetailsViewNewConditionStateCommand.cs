using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewNewConditionStateCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedCondition != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "GetProjectStates";
            var dialogModel = new StateDialogModel
            {
                DecisionTableManagerId = contextViewModel.DecisionTableManagerId,
                ExistingStates = ViewModelService.Instance.ExecuteOperation<List<StateViewModel>>(serviceId, operationId, contextViewModel.DecisionTableManagerId)
            };
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                string ownerCollection = "ValidStates";
                var selectedStates = dialogModel.ExistingStates.Where(cur => cur.IsSelected);
                if (selectedStates.Count() > 0)
                {
                    HistoryService.Instance.BeginSession();
                    ViewModelService.Instance.InsertViewModels(selectedStates, contextViewModel.SelectedCondition.EntityId, ownerCollection);
                    HistoryService.Instance.EndSession();
                }
            }

            string serviceId2 = "DTServices.CommonServices";
            string operationId2 = "UpdateProjectStates";
            ViewModelService.Instance.ExecuteOperation(serviceId2, operationId2, contextViewModel.DecisionTableManagerId);
        }
    }
}
