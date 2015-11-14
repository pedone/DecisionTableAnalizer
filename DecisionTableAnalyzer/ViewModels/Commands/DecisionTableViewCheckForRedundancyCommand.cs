using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class DecisionTableViewCheckForRedundancyCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            List<RuleViewModel> redundantRules = null;
            DecisionTableViewModelUtils.Instance.CheckForRedundancy(contextViewModel, out redundantRules);
            var dialogModel = new CheckForRedundancyDialogModel
            {
                RedundantRulesCount = redundantRules.Count
            };

            if (ViewService.Instance.ShowDialog(dialogModel))
            {
                var removeRedundantRulesDialog = new RemoveRedundantRulesDialogModel
                {
                    Conditions = contextViewModel.Conditions,
                    Actions = contextViewModel.Actions,
                    Rules = redundantRules
                };
                removeRedundantRulesDialog.BuildRows();

                bool result = ViewService.Instance.ShowDialog(removeRedundantRulesDialog);
                var selectedRedundantRules = removeRedundantRulesDialog.Rules.Where(cur => cur.IsSelected);
                if (result)
                {
                    HistoryService.Instance.BeginSession();

                    var resultRules = contextViewModel.Rules.Except(selectedRedundantRules).ToList();
                    contextViewModel.Rules = resultRules;
                    ViewModelService.Instance.CommitViewModel(contextViewModel);

                    HistoryService.Instance.EndSession();
                }
                else
                {
                    foreach (var rule in selectedRedundantRules)
                        rule.IsSelected = false;
                }
            }
        }
    }
}
