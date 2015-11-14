using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class DecisionTableViewAddMissingRulesCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            List<RuleViewModel> missingRules = null;
            DecisionTableViewModelUtils.Instance.CheckForCompleteness(contextViewModel, out missingRules);
            var dialogModel = new AddMissingRulesDialogModel
            {
                Conditions = contextViewModel.Conditions,
                Actions = contextViewModel.Actions,
                Rules = missingRules
            };
            dialogModel.BuildRows();

            bool result = ViewService.Instance.ShowDialog(dialogModel);
            var selectedMissingRules = dialogModel.Rules.Where(cur => cur.IsSelected);
            if (result)
            {
                var combinedRules = DecisionTableViewModelUtils.Instance.AddRulesToEnd(contextViewModel, selectedMissingRules);
                contextViewModel.Rules = combinedRules;

                HistoryService.Instance.BeginSession();
                ViewModelService.Instance.CommitViewModel(contextViewModel);
                HistoryService.Instance.EndSession();
            }
            else
            {
                foreach (var rule in selectedMissingRules)
                    rule.IsSelected = false;
            }
        }

    }
}
