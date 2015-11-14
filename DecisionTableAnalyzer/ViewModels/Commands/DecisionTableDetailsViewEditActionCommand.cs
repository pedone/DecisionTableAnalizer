using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewEditActionCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedAction != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            var dialogModel = ViewModelService.Instance.QueryViewModel<ActionDialogModel>(contextViewModel.SelectedAction.EntityId);
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();
                ViewModelService.Instance.CommitViewModel(dialogModel);

                if (contextViewModel.SelectedAction.HasReferenceSubTable)
                {
                    string serviceId = "DTServices.DecisionTableDetailServices";
                    string operationId = "SynchronizeSubDecisionTableWithReferenceElement";
                    ViewModelService.Instance.ExecuteOperation(serviceId, operationId, contextViewModel.SelectedAction.EntityId);
                }

                HistoryService.Instance.EndSession();
            }
        }
    }
}
