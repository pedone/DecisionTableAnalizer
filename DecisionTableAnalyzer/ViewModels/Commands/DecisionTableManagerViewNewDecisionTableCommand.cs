using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;
using ViewModels;

namespace ViewModels.Commands
{
    public class DecisionTableManagerViewNewDecisionTableCommand : ViewModelCommand<DecisionTableManagerViewModel>
    {

        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return true;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            var dialogModel = new DecisionTableDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();

                dialogModel.DecisionTableManagerId = contextViewModel.EntityId;
                string ownerCollection = "DecisionTables";
                ViewModelService.Instance.InsertViewModel(dialogModel, contextViewModel.EntityId, ownerCollection);

                HistoryService.Instance.EndSession();
            }
        }

    }
}
