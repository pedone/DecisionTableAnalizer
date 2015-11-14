using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class DecisionTableViewCheckForContradictionCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            List<ContradictionGroup> contradictedRules = null;
            DecisionTableViewModelUtils.Instance.CheckForContradiction(contextViewModel, out contradictedRules);
            var dialogModel = new CheckForContradictionDialogModel
            {
                ContradictionCount = contradictedRules.Count
            };

            if (ViewService.Instance.ShowDialog(dialogModel))
            {
                var contradictionTable = BuildContradictionTable(contextViewModel.Rules, contradictedRules);
                var removeContradictedRulesDialog = new RemoveContradictedRulesDialogModel
                {
                    Conditions = contextViewModel.Conditions,
                    Actions = contextViewModel.Actions,
                    Rules = contextViewModel.Rules,
                    ContradictionTable = contradictionTable,
                    ContradictionGroups = contradictedRules
                };
                removeContradictedRulesDialog.BuildRows();

                bool result = ViewService.Instance.ShowDialog(removeContradictedRulesDialog);
                var selectedContradictedRules = removeContradictedRulesDialog.Rules.Where(cur => cur.IsSelected);
                if (result)
                {
                    HistoryService.Instance.BeginSession();

                    var resultRules = contextViewModel.Rules.Except(selectedContradictedRules).ToList();
                    resultRules = DecisionTableViewModelUtils.Instance.ReindexRules(resultRules);
                    contextViewModel.Rules = resultRules;
                    ViewModelService.Instance.CommitViewModel(contextViewModel);

                    HistoryService.Instance.EndSession();
                }
                else
                {
                    foreach (var rule in selectedContradictedRules)
                        rule.IsSelected = false;
                }
            }
        }

        private List<ContradictionViewModel> BuildContradictionTable(List<RuleViewModel> rules, List<ContradictionGroup> contradictedRules)
        {
            var contradictionTable = new List<ContradictionViewModel>();
            var emptyContradictionStates = Enumerable.Repeat<bool?>(null, rules.Count);
            foreach (var rule in rules)
            {
                contradictionTable.Add(new ContradictionViewModel
                {
                    Rules = rules,
                    Header = rule,
                    ContradictionStates = emptyContradictionStates.ToList()
                });
            }

            //init left side of the table with false
            for (int x = 0; x < rules.Count; x++)
                for (int y = x + 1; y < rules.Count; y++)
                {
                    contradictionTable[y].ContradictionStates[x] = false;
                }

            //insert contradicted states
            foreach (var contradictedPair in contradictedRules)
            {
                int xIndex = Math.Min(contradictedPair.RuleA.Index, contradictedPair.RuleB.Index);
                int yIndex = Math.Max(contradictedPair.RuleA.Index, contradictedPair.RuleB.Index);

                contradictionTable[yIndex].ContradictionStates[xIndex] = true;
            }

            return contradictionTable;
        }
    }
}
