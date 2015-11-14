using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ExtensionLibrary;

namespace ViewModels.Utils
{
    public class DecisionTableViewModelUtils
    {

        public static DecisionTableViewModelUtils Instance { get; private set; }

        static DecisionTableViewModelUtils()
        {
            Instance = new DecisionTableViewModelUtils();
        }

        private DecisionTableViewModelUtils()
        { }

        private int CalculateRuleCount(IEnumerable<ConditionViewModel> conditions)
        {
            if (conditions.Count() == 0)
                return 0;

            int ruleCount = 1;
            foreach (var condition in conditions)
                ruleCount *= condition.ValidStates.Count;

            return ruleCount;
        }

        private Dictionary<ConditionViewModel, List<StateViewModel>> CalculateConditionStates(IEnumerable<ConditionViewModel> conditions)
        {
            var conditionData = new Dictionary<ConditionViewModel, List<StateViewModel>>();
            int ruleCount = CalculateRuleCount(conditions);

            //Conditions
            int multipliedStateCount = 1;
            for (int i = 0; i < conditions.Count(); i++)
            {
                var curCondition = conditions.ElementAt(i);

                //how many states in a row, e.g.
                //true true true true false false false false -> 4
                //true true false false true true false false -> 2
                //true false true false true false true false -> 1
                multipliedStateCount *= curCondition.ValidStates.Count();
                int consecutiveRowCount = (int)(ruleCount / multipliedStateCount);

                var conditionStates = new List<StateViewModel>();
                int curStateIndex = 0;
                for (int column = 0; column < ruleCount; column++)
                {
                    conditionStates.Add(curCondition.ValidStates.ElementAt(curStateIndex));

                    if ((column + 1) % consecutiveRowCount == 0)
                    {
                        curStateIndex++;
                        if (curStateIndex >= curCondition.ValidStates.Count())
                            curStateIndex = 0;
                    }
                }

                conditionData.Add(curCondition, conditionStates);
            }

            return conditionData;
        }

        public bool Simplify(DecisionTableViewModel decisionTable, out List<RuleViewModel> simplifiedRules)
        {
            bool couldSimplify = false;
            bool mergeSucceded = false;

            var conditionNoPreferenceState = GetConditionNoPreferenceState(decisionTable);
            var simplifiedRulesInternal = decisionTable.Rules.ToList();
            var priorities = decisionTable.Conditions.Select((condition, index) => new { EntityId = condition.EntityId, Priority = index });
            Func<ConditionViewModel, int> getPriorityFunction = condition => priorities.FirstOrDefault(cur => cur.EntityId.Equals(condition.EntityId)).Priority;

            do
            {
                mergeSucceded = false;
                //Group rules by action pattern
                var rulesGroupedByActionPattern = from rule in simplifiedRulesInternal
                                                  let actionPattern = (from actionPair in rule.ActionStates
                                                                       select actionPair.Value)
                                                  group rule by actionPattern
                                                      into ruleGroup
                                                      select (from subRule in simplifiedRulesInternal
                                                              let subRuleActionPattern = (from actionPair in subRule.ActionStates
                                                                                          select actionPair.Value)
                                                              where subRuleActionPattern.SequenceEqual(ruleGroup.Key)
                                                              select subRule);

                var ruleGroupsEqualityComparer = new EqualityComparerDelegate<IEnumerable<RuleViewModel>>(
                    (rulesA, rulesB) => rulesA.SequenceEqual(rulesB),
                    (ruleList) =>
                    {
                        int hashCode = 0;
                        ruleList.ToList().ForEach(cur => hashCode += cur.GetHashCode());
                        return hashCode;
                    });
                rulesGroupedByActionPattern = rulesGroupedByActionPattern.Distinct(ruleGroupsEqualityComparer);

                //Make pairs of rules that could be merged
                var validMergePairs = from ruleGroup in rulesGroupedByActionPattern
                                      select (from rule in ruleGroup
                                              let validMergeRules = (from subRule in ruleGroup
                                                                     where subRule != rule && CompareConditions(rule, subRule).Count == 1
                                                                     select subRule)
                                              select (from validMergeRule in validMergeRules
                                                      select new { Rule1 = rule, Rule2 = validMergeRule }));

                //because the validMergPairs are nested, flatten them
                var flatValidMergePairs = validMergePairs.SelectMany(cur1 => cur1.SelectMany(cur2 => cur2));

                //If a rule is used more than once, use the one with the lowest priority and discard the other
                var distinctMergePairs = (from pair in flatValidMergePairs
                                          let allPairsContainingEitherRule = flatValidMergePairs.Where(cur => cur.Rule1 == pair.Rule1 || cur.Rule2 == pair.Rule1 || cur.Rule2 == pair.Rule1 || cur.Rule2 == pair.Rule2)
                                          where allPairsContainingEitherRule.Where(cur => cur != pair).Count() > 0
                                          let maxMergeElementPriority = allPairsContainingEitherRule.Max(cur =>
                                          {
                                              var curMergeElement = CompareConditions(cur.Rule1, cur.Rule2).First().RuleElement;
                                              return getPriorityFunction(curMergeElement);
                                          })
                                          let curMergeElement = CompareConditions(pair.Rule1, pair.Rule2).First().RuleElement
                                          let curMergeElementPriority = getPriorityFunction(curMergeElement)
                                          where curMergeElementPriority == maxMergeElementPriority
                                          select pair)
                                          //All pairs are in there twice, e.g. pair1.Rule1 = a, pair1.Rule2 = b / pair1.Rule1 = b, pair1.Rule2 = a
                                          //make sure to only get the first
                                          .GroupBy(cur => cur.Rule1.GetHashCode() + cur.Rule2.GetHashCode())
                                          .Select(cur => cur.First());

                //Merge the pairs
                foreach (var mergePair in distinctMergePairs)
                {
                    //mergePair.Rule1.Merge(mergePair.Rule2);
                    var mergedRule = MergeRules(mergePair.Rule1, mergePair.Rule2, conditionNoPreferenceState);
                    simplifiedRulesInternal.Remove(mergePair.Rule1);
                    simplifiedRulesInternal.Remove(mergePair.Rule2);
                    simplifiedRulesInternal.Add(mergedRule);

                    if (!mergeSucceded)
                        mergeSucceded = true;
                }

                if (!couldSimplify && mergeSucceded)
                    couldSimplify = true;

            } while (mergeSucceded);

            simplifiedRules = simplifiedRulesInternal;
            return couldSimplify;
        }

        public bool CheckForRedundancy(DecisionTableViewModel decisionTable, out List<RuleViewModel> redundantRules)
        {
            var actionEmptyState = GetActionEmptyState(decisionTable);
            var conditonNoPreferenceState = GetConditionNoPreferenceState(decisionTable);

            var rulesWithEmptyConditionStates = (from rule in decisionTable.Rules
                                                 where rule.ConditionStates.Any(cur => cur.Value.EntityId.Equals(conditonNoPreferenceState.EntityId))
                                                 select rule);

            var redundantRulesSet = new HashSet<RuleViewModel>();
            //All rules with same state where not empty and state where empty are redundant
            foreach (var rule in rulesWithEmptyConditionStates)
            {
                //continue if the current rule was already declared redundant by a rule with the same or more empty states
                if (redundantRulesSet.Any(cur => cur.EntityId.Equals(rule.EntityId)))
                    continue;

                //Get all rules that have same states where current rule has not-empty state
                var subsetRules = from curSubRule in decisionTable.Rules
                                  where !curSubRule.EntityId.Equals(rule.EntityId)
                                  let differingConditionStateList = (from resultContainer in CompareConditions(rule, curSubRule)
                                                                     select resultContainer.StateA)
                                  let noDifferingActionStates = CompareActions(rule, curSubRule).Count == 0
                                  let noDifferingConditionStatesOrEmpty = differingConditionStateList.All(cur => cur.EntityId.Equals(conditonNoPreferenceState.EntityId))
                                  where noDifferingActionStates && noDifferingConditionStatesOrEmpty
                                  select curSubRule;

                //since al the subRules are included in the current main rule, the sub rules are redundant
                foreach (var element in subsetRules)
                    redundantRulesSet.Add(element);
            }

            //Get all rules with same states that are not yet declared redundant
            var redundantDuplicateRules = from rule in decisionTable.Rules
                                          where rule.ConditionStates.All(cur => cur.Value.EntityId != conditonNoPreferenceState.EntityId)
                                          let curDuplicates = decisionTable.Rules.Where(cur => CompareConditions(rule, cur).Count == 0 && CompareActions(rule, cur).Count == 0)
                                          let ruleWithSmallestIndex = curDuplicates.OrderBy(cur => cur.Index).FirstOrDefault()
                                          select curDuplicates.Except(new[] { ruleWithSmallestIndex });

            //Mark the last of the duplicates as redundant
            foreach (var ruleList in redundantDuplicateRules)
            {
                foreach (var rule in ruleList)
                    redundantRulesSet.Add(rule);
            }

            redundantRules = redundantRulesSet.ToList();
            return redundantRules.Count() > 0;
        }

        public bool CheckForContradiction(DecisionTableViewModel decisionTable, out List<ContradictionGroup> contradictedRules)
        {
            var actionEmptyState = GetActionEmptyState(decisionTable);
            var conditonNoPreferenceState = GetConditionNoPreferenceState(decisionTable);

            var manualRuleGroups = new Dictionary<RuleViewModel, List<RuleViewModel>>();
            foreach (var rule in decisionTable.Rules)
            {
                var curGroup = from curRule in decisionTable.Rules
                               where curRule != rule
                               let noDifferingStates = CompareConditions(curRule, rule).Count == 0
                               let differingStatesAreEmpty = CompareConditions(curRule, rule)
                                                               .All(comparisonResult => comparisonResult.StateA.EntityId == conditonNoPreferenceState.EntityId || comparisonResult.StateB.EntityId == conditonNoPreferenceState.EntityId)
                               where differingStatesAreEmpty || noDifferingStates
                               select curRule;

                foreach (var curRule in decisionTable.Rules)
                {
                    if (curRule != rule)
                    {
                        var noDifferingStates = CompareConditions(curRule, rule).Count == 0;
                        var differingStatesAreEmpty = CompareConditions(curRule, rule)
                                                        .All(comparisonResult => comparisonResult.StateA.EntityId == conditonNoPreferenceState.EntityId || comparisonResult.StateB.EntityId == conditonNoPreferenceState.EntityId);
                        if (differingStatesAreEmpty || noDifferingStates)
                        {
                        }
                    }
                }

                if (curGroup.Count() > 0)
                    manualRuleGroups.Add(rule, curGroup.ToList());
            }

            var contradictedRulesInternal = new List<ContradictionGroup>();
            foreach (var ruleGroup in manualRuleGroups)
            {
                var contradictedRuleGroups = new List<ContradictionGroup>();
                var rule = ruleGroup.Key;
                foreach (var otherRule in ruleGroup.Value)
                {
                    bool isContradicted = CompareActions(rule, otherRule).Count > 0;
                    if (isContradicted)
                    {
                        var newContradictionGroup = new ContradictionGroup { RuleA = rule, RuleB = otherRule };
                        if (!contradictedRuleGroups.Any(cur => cur.IsEquivalent(newContradictionGroup)) &&
                            !contradictedRulesInternal.Any(cur => cur.IsEquivalent(newContradictionGroup)))
                        {
                            contradictedRuleGroups.Add(newContradictionGroup);
                        }
                    }
                }

                if (contradictedRuleGroups.Count > 0)
                    contradictedRulesInternal.AddRange(contradictedRuleGroups);
            }

            contradictedRules = contradictedRulesInternal;
            return contradictedRules.Count() > 0;
        }

        private RuleViewModel MergeRules(RuleViewModel ruleA, RuleViewModel ruleB, StateViewModel conditionNoPreferenceState)
        {
            var mergedRule = new RuleViewModel
            {
                Index = ruleA.Index,
                ConditionStates = new Dictionary<ConditionViewModel, StateViewModel>(ruleA.ConditionStates),
                ActionStates = new Dictionary<ActionViewModel, StateViewModel>(ruleA.ActionStates)
            };
            foreach (var differenceInfo in CompareConditions(mergedRule, ruleB))
            {
                var differentCondition = mergedRule.ConditionStates.Keys.First(cur => cur.EntityId.Equals(differenceInfo.RuleElement.EntityId));
                mergedRule.ConditionStates[differentCondition] = conditionNoPreferenceState;
            }

            return mergedRule;
        }

        public List<RuleViewModel> AddRulesToEnd(DecisionTableViewModel decisionTable, IEnumerable<RuleViewModel> rules)
        {
            int nextIndex = decisionTable.Rules.Count > 0 ? decisionTable.Rules.Max(cur => cur.Index) + 1 : 0;
            var combinedRules = decisionTable.Rules.ToList();
            foreach (var newRule in rules)
            {
                newRule.Index = nextIndex++;
                combinedRules.Add(newRule);
            }

            return combinedRules;
        }

        public bool CheckForCompleteness(DecisionTableViewModel decisionTable, out List<RuleViewModel> missingRules)
        {
            var conditionNoPreferenceState = GetConditionNoPreferenceState(decisionTable);
            var expectedRuleCount = CalculateRuleCount(decisionTable.Conditions);
            var allPossibleRules = BuildRules(decisionTable);

            var missingRulesList = allPossibleRules.ToList();
            foreach (var rule in allPossibleRules)
            {
                if (decisionTable.Rules.Any(curRule =>
                {
                    var differentStates = CompareConditions(rule, curRule);
                    return differentStates.Count == 0 ||
                        differentStates.All(cur =>
                            {
                                return cur.StateA.EntityId.Equals(conditionNoPreferenceState.EntityId) ||
                                    cur.StateB.EntityId.Equals(conditionNoPreferenceState.EntityId);
                            });
                }))
                {
                    missingRulesList.Remove(rule);
                }
            }

            missingRules = missingRulesList;
            return missingRules.Count() == 0;
        }

        private List<RuleComparerResultContainer<ConditionViewModel>> CompareConditions(RuleViewModel ruleA, RuleViewModel ruleB)
        {
            if (ruleA == null || ruleB == null || ruleA == ruleB)
                return new List<RuleComparerResultContainer<ConditionViewModel>>();

            var result = new List<RuleComparerResultContainer<ConditionViewModel>>();
            foreach (var conditionPairA in ruleA.ConditionStates)
            {
                var curConditionId = conditionPairA.Key.EntityId;
                var stateA = conditionPairA.Value;
                var matchingPairB = ruleB.ConditionStates.FirstOrDefault(cur => cur.Key.EntityId.Equals(curConditionId));
                if (matchingPairB.Equals(default(KeyValuePair<ConditionViewModel, StateViewModel>)))
                    continue;

                var stateB = matchingPairB.Value;
                if (!stateA.EntityId.Equals(stateB.EntityId))
                    result.Add(new RuleComparerResultContainer<ConditionViewModel>(conditionPairA.Key, stateA, stateB));
            }

            return result;
        }

        private List<RuleComparerResultContainer<ActionViewModel>> CompareActions(RuleViewModel ruleA, RuleViewModel ruleB)
        {
            if (ruleA == null || ruleB == null || ruleA == ruleB)
                return new List<RuleComparerResultContainer<ActionViewModel>>();

            var result = new List<RuleComparerResultContainer<ActionViewModel>>();
            foreach (var actionPairA in ruleA.ActionStates)
            {
                var curActionId = actionPairA.Key.EntityId;
                var stateA = actionPairA.Value;
                var matchingPairB = ruleB.ActionStates.First(cur => cur.Key.EntityId.Equals(curActionId));
                var stateB = matchingPairB.Value;
                if (!stateA.EntityId.Equals(stateB.EntityId))
                    result.Add(new RuleComparerResultContainer<ActionViewModel>(actionPairA.Key, stateA, stateB));
            }

            return result;
        }

        public List<RuleViewModel> BuildRules(DecisionTableViewModel decisionTable)
        {
            //Conditions
            var conditionData = CalculateConditionStates(decisionTable.Conditions);

            //Actions
            var emptyState = GetActionEmptyState(decisionTable);
            int ruleCount = CalculateRuleCount(decisionTable.Conditions);
            var actionData = new Dictionary<ActionViewModel, List<StateViewModel>>();
            foreach (var action in decisionTable.Actions)
                actionData.Add(action, Enumerable.Repeat(emptyState, ruleCount).ToList());

            var rules = new List<RuleViewModel>();
            for (int i = 0; i < ruleCount; i++)
            {
                rules.Add(new RuleViewModel
                {
                    Index = i,
                    ConditionStates = conditionData.ToDictionary(pair => pair.Key, pair => pair.Value[i]),
                    ActionStates = actionData.ToDictionary(pair => pair.Key, pair => pair.Value[i]),
                });
            }

            return rules;
        }

        private StateViewModel GetActionEmptyState(DecisionTableViewModel decisionTable)
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "GetActionEmptyState";
            return ViewModelService.Instance.ExecuteOperation<StateViewModel>(serviceId, operationId, decisionTable.DecisionTableManagerId);
        }

        private StateViewModel GetConditionNoPreferenceState(DecisionTableViewModel decisionTable)
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "GetConditionNoPreferenceState";
            return ViewModelService.Instance.ExecuteOperation<StateViewModel>(serviceId, operationId, decisionTable.DecisionTableManagerId);
        }

        public List<RuleViewModel> ReindexRules(IEnumerable<RuleViewModel> rules)
        {
            if (rules == null)
                return new List<RuleViewModel>();

            var indexdRules = rules.OrderBy(cur => cur.Index).ToList();
            for (int i = 0; i < indexdRules.Count; i++)
                indexdRules[i].Index = i;

            return indexdRules;
        }

        public bool ExtendRules(DecisionTableViewModel decisionTable, out List<RuleViewModel> extendedRules)
        {
            var actionEmptyState = GetActionEmptyState(decisionTable);
            var conditonNoPreferenceState = GetConditionNoPreferenceState(decisionTable);

            //Get rules with empty condition states
            var extendedRulesInternal = decisionTable.Rules.ToList();
            var rulesWithEmptyConditionStates = (from rule in extendedRulesInternal
                                                 where rule.ConditionStates.Any(cur => cur.Value.EntityId.Equals(conditonNoPreferenceState.EntityId))
                                                 select rule);

            if (rulesWithEmptyConditionStates.Count() == 0)
            {
                extendedRules = new List<RuleViewModel>();
                return false;
            }

            while (rulesWithEmptyConditionStates.Count() > 0)
            {
                //Foreach empty condition state (top to bottom), create rules with each possible state instead of empty
                foreach (var rule in rulesWithEmptyConditionStates.ToList())
                {
                    var conditionWithEmptyState = rule.ConditionStates.First(cur => cur.Value.EntityId.Equals(conditonNoPreferenceState.EntityId)).Key;
                    foreach (var state in conditionWithEmptyState.ValidStates)
                    {
                        var conditionStates = new Dictionary<ConditionViewModel, StateViewModel>(rule.ConditionStates);
                        conditionStates[conditionWithEmptyState] = state;
                        extendedRulesInternal.Add(new RuleViewModel
                        {
                            Index = rule.Index,
                            ConditionStates = conditionStates,
                            ActionStates = rule.ActionStates
                        });
                    }

                    extendedRulesInternal.Remove(rule);
                }
            }

            extendedRules = extendedRulesInternal;
            return true;
        }

        private class RuleComparerResultContainer
        {
            public StateViewModel StateA { get; private set; }
            public StateViewModel StateB { get; private set; }

            public RuleComparerResultContainer(StateViewModel stateA, StateViewModel stateB)
            {
                StateA = stateA;
                StateB = stateB;
            }
        }

        private class RuleComparerResultContainer<RuleElementType> : RuleComparerResultContainer
        {
            public RuleElementType RuleElement { get; private set; }

            public RuleComparerResultContainer(RuleElementType ruleElement, StateViewModel stateA, StateViewModel stateB)
                : base(stateA, stateB)
            {
                RuleElement = ruleElement;
            }
        }

        private class EqualityComparerDelegate<T> : IEqualityComparer<T>
        {

            public Func<T, T, bool> Comparer { get; private set; }
            public Func<T, int> GetHashCodeFunction { get; private set; }

            public EqualityComparerDelegate(Func<T, T, bool> comparer, Func<T, int> getHashCodeFunction)
            {
                Comparer = comparer;
                GetHashCodeFunction = getHashCodeFunction;
            }

            public EqualityComparerDelegate(Func<T, T, bool> comparer)
                : this(comparer, null)
            { }

            public bool Equals(T x, T y)
            {
                if (Comparer != null)
                    return Comparer(x, y);

                return false;
            }

            public int GetHashCode(T obj)
            {
                if (GetHashCodeFunction != null)
                    return GetHashCodeFunction(obj);

                return obj.GetHashCode();
            }
        }

    }

}