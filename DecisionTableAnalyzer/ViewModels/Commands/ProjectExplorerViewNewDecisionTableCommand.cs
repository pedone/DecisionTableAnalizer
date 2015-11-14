using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewNewDecisionTableCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var dialogModel = new DecisionTableDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                string serviceId = "DTServices.CommonServices";
                string operationId = "GetDecisionTableManager";
                var decisionTableManagerViewModel = ViewModelService.Instance.ExecuteOperation<DecisionTableManagerViewModel>(serviceId, operationId, contextViewModel.Project.EntityId);

                HistoryService.Instance.BeginSession();

                dialogModel.DecisionTableManagerId = decisionTableManagerViewModel.EntityId;
                string ownerCollection = "DecisionTables";
                ViewModelService.Instance.InsertViewModel(dialogModel, decisionTableManagerViewModel.EntityId, ownerCollection);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
