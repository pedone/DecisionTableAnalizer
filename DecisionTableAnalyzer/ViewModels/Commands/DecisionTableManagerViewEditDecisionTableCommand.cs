using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableManagerViewEditDecisionTableCommand : ViewModelCommand<DecisionTableManagerViewModel>
    {
        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedDecisionTable != null;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            var dialogModel = ViewModelService.Instance.QueryViewModel<DecisionTableDialogModel>(contextViewModel.SelectedDecisionTable.EntityId);
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();
                ViewModelService.Instance.CommitViewModel(dialogModel);
                HistoryService.Instance.EndSession();
            }
        }
    }
}
