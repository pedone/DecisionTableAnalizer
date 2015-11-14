using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewNewConditionCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemDecisionTableViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var dialogModel = new ConditionDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();

                dialogModel.DecisionTableId = contextViewModel.SelectedElement.EntityId;
                string ownerCollection = "Conditions";
                ViewModelService.Instance.InsertViewModel(dialogModel, contextViewModel.SelectedElement.EntityId, ownerCollection);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
