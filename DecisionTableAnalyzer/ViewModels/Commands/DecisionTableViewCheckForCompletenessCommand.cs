using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class DecisionTableViewCheckForCompletenessCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            List<RuleViewModel> missingRules = null;
            DecisionTableViewModelUtils.Instance.CheckForCompleteness(contextViewModel, out missingRules);
            var dialogModel = new CheckForCompletenessDialogModel
            {
                MissingRulesCount = missingRules.Count
            };

            if (ViewService.Instance.ShowDialog(dialogModel))
            {
                var addMissingRulesModel = new AddMissingRulesDialogModel
                {
                    Conditions = contextViewModel.Conditions,
                    Actions = contextViewModel.Actions,
                    Rules = missingRules
                };
                addMissingRulesModel.BuildRows();

                bool result = ViewService.Instance.ShowDialog(addMissingRulesModel);
                var selectedMissingRules = addMissingRulesModel.Rules.Where(cur => cur.IsSelected);
                if (result)
                {
                    HistoryService.Instance.BeginSession();
                    
                    var combinedRules = DecisionTableViewModelUtils.Instance.AddRulesToEnd(contextViewModel, selectedMissingRules);
                    contextViewModel.Rules = combinedRules;
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
}
