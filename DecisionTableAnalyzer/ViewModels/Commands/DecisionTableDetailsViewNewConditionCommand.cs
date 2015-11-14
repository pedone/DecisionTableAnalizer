using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewNewConditionCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return true;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            var dialogModel = new ConditionDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();

                dialogModel.DecisionTableId = contextViewModel.EntityId;
                string ownerCollection = "Conditions";
                ViewModelService.Instance.InsertViewModel(dialogModel, contextViewModel.EntityId, ownerCollection);

                HistoryService.Instance.EndSession();
            }
        }
    }
}
