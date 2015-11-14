using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableManagerViewRemoveDecisionTableCommand : ViewModelCommand<DecisionTableManagerViewModel>
    {
        public override bool CanExecute(DecisionTableManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedDecisionTable != null;
        }

        public override void Execute(DecisionTableManagerViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();
            ViewModelService.Instance.DeleteViewModel(contextViewModel.SelectedDecisionTable);
            HistoryService.Instance.EndSession();
        }
    }
}
