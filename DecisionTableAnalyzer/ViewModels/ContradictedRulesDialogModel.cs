using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;

namespace ViewModels
{
    public class ContradictedRulesDialogModel : ViewModel
    {

        public class SelectableRuleContainer
        {
            public DTRule Rule { get; private set; }
            public bool IsSelected { get; set; }

            public SelectableRuleContainer(DTRule rule)
            {
                Rule = rule;
            }
        }

        public List<List<SelectableRuleContainer>> ContradictedRules { get; private set; }
        public IEnumerable<DTRule> RulesToRemove
        {
            get
            {
                return ContradictedRules.SelectMany(ruleGroup => from rule in ruleGroup
                                                                 where rule.IsSelected
                                                                 select rule.Rule).Distinct();
            }
        }

        public ContradictedRulesDialogModel(IEnumerable<IEnumerable<DTRule>> contradictedRuleGroups)
        {
            ContradictedRules = new List<List<SelectableRuleContainer>>();
            foreach (var ruleGroup in contradictedRuleGroups)
            {
                ContradictedRules.Add(new List<SelectableRuleContainer>(from rule in ruleGroup
                                                                        let existingContainer = (from existingGroup in ContradictedRules
                                                                                                 from existingContainer in existingGroup
                                                                                                 where existingContainer.Rule == rule
                                                                                                 select existingContainer).FirstOrDefault()
                                                                        select existingContainer ?? new SelectableRuleContainer(rule)));
            }
        }

    }
}
