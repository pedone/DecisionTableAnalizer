using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableDetailsViewNewActionCommand : ViewModelCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return true;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            var dialogModel = new ActionDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();

                dialogModel.DecisionTableId = contextViewModel.EntityId;
                string ownerCollection = "Actions";
                ViewModelService.Instance.InsertViewModel(dialogModel, contextViewModel.EntityId, ownerCollection);
                
                HistoryService.Instance.EndSession();
            }
        }
    }
}
