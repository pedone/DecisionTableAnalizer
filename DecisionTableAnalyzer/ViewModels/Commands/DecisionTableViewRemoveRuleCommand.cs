using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableViewRemoveRuleCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedRule != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();

            ViewModelService.Instance.DeleteViewModel(contextViewModel.SelectedRule);
            contextViewModel.SelectedRuleIndex = -1;

            //Reindex rules
            var sortedRules = contextViewModel.Rules.OrderBy(cur => cur.Index).ToList();
            for (int i = 0; i < sortedRules.Count(); i++)
                sortedRules[i].Index = i;

            ViewModelService.Instance.CommitViewModel(contextViewModel);

            HistoryService.Instance.EndSession();
        }

    }
}
