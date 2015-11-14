using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using DTCore;
using ViewDatas;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ViewModels
{
    [ViewDataType(typeof(DecisionTableViewData))]
    public class DecisionTableViewModel : ViewModel<DecisionTableViewData>
    {

        public int SelectedRuleIndex { get; set; }

        public RuleViewModel SelectedRule
        {
            get { return Rules.ElementAtOrDefault(SelectedRuleIndex); }
        }

        private EntityId _DecisionTableManagerId;
        public EntityId DecisionTableManagerId
        {
            get { return _DecisionTableManagerId; }
            set
            {
                _DecisionTableManagerId = value;
                NotifyPropertyChanged<EntityId>(() => DecisionTableManagerId);
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged<string>(() => Name);
            }
        }

        private List<RowViewModel> _Rows;
        public List<RowViewModel> Rows
        {
            get { return _Rows; }
            set
            {
                _Rows = value;
                NotifyPropertyChanged<List<RowViewModel>>(() => Rows);
            }
        }

        private List<RuleViewModel> _Rules;
        public List<RuleViewModel> Rules
        {
            get { return _Rules; }
            set
            {
                _Rules = value;
                NotifyPropertyChanged<List<RuleViewModel>>(() => Rules);
            }
        }

        private List<ActionViewModel> _Actions;
        public List<ActionViewModel> Actions
        {
            get { return _Actions; }
            set
            {
                _Actions = value;
                NotifyPropertyChanged<List<ActionViewModel>>(() => Actions);
            }
        }

        private List<ConditionViewModel> _Conditions;
        public List<ConditionViewModel> Conditions
        {
            get { return _Conditions; }
            set
            {
                _Conditions = value;
                NotifyPropertyChanged<List<ConditionViewModel>>(() => Conditions);
            }
        }

        public override void CopyFromViewData(DecisionTableViewData viewData)
        {
            Name = viewData.Name;
            DecisionTableManagerId = viewData.DecisionTableManagerId;

            Rules = CopyViewModelsFromViewDatas<RuleViewData, RuleViewModel>(viewData.Rules);
            Conditions = CopyViewModelsFromViewDatas<ConditionViewData, ConditionViewModel>(viewData.Conditions);
            Actions = CopyViewModelsFromViewDatas<ActionViewData, ActionViewModel>(viewData.Actions);

            Rows = BuildRows();
            AttachToRules();
        }

        /// <summary>
        /// The rows need to be updated when a rule changes, so attach to the rules
        /// </summary>
        private void AttachToRules()
        {
            foreach(var rule in Rules)
                rule.Updated += Rule_Updated;
        }

        private void Rule_Updated(ViewModel viewModel)
        {
            Rows = BuildRows();
        }

        public override void CopyToViewData(DecisionTableViewData viewData)
        {
            viewData.Name = Name;
            viewData.Rules = Rules.Select(cur => CreateViewData<RuleViewData>(cur)).ToList();
        }

        private List<RowViewModel> BuildRows()
        {
            var rows = new List<RowViewModel>();

            var sortedRules = Rules.OrderBy(cur => cur.Index);
            foreach (var condition in Conditions)
            {
                var curRow = new RowViewModel
                {
                    Header = condition,
                    States = new ObservableCollection<StateViewModel>()
                };
                foreach (var rule in sortedRules)
                {
                    var conditionStatePairList = rule.ConditionStates.Where(cur => cur.Key.EntityId.Equals(condition.EntityId));
                    if (conditionStatePairList.Count() > 0)
                    {
                        var conditionStatePair = conditionStatePairList.ElementAt(0);
                        curRow.States.Add(conditionStatePair.Value);
                    }
                }

                curRow.States.CollectionChanged += (sender, e) => States_CollectionChanged(curRow, e);
                rows.Add(curRow);
            }

            foreach (var action in Actions)
            {
                var curRow = new RowViewModel
                {
                    Header = action,
                    States = new ObservableCollection<StateViewModel>()
                };
                foreach (var rule in sortedRules)
                {
                    var actionStatePairList = rule.ActionStates.Where(cur => cur.Key.EntityId.Equals(action.EntityId));
                    if (actionStatePairList.Count() > 0)
                    {
                        var actionStatePair = actionStatePairList.ElementAt(0);
                        curRow.States.Add(actionStatePair.Value);
                    }
                }

                curRow.States.CollectionChanged += (sender, e) => States_CollectionChanged(curRow, e);
                rows.Add(curRow);
            }

            return rows;
        }

        private void States_CollectionChanged(RowViewModel row, NotifyCollectionChangedEventArgs e)
        {
            var newItemList = e.NewItems.OfType<StateViewModel>();
            var oldItemList = e.OldItems.OfType<StateViewModel>();
            bool areEqual = e.NewItems.Count == e.OldItems.Count &&
                newItemList.All(curNew => oldItemList.Any(curOld => curOld.EntityId.Equals(curNew.EntityId)));

            if (!areEqual)
            {
                var sortedRules = Rules.OrderBy(cur => cur.Index).ToList();
                for (int i = 0; i < sortedRules.Count; i++)
                {
                    var curRule = sortedRules[i];
                    var newState = row.States[i];
                    if (row.Header is ConditionViewModel)
                    {
                        //Remove the existing conditionState first, because comparing them internally won't work right since they're different instances
                        var existingConditionWithSameId = curRule.ConditionStates.Keys.FirstOrDefault(cur => cur.EntityId.Equals(row.Header.EntityId));
                        if (existingConditionWithSameId != null)
                            curRule.ConditionStates.Remove(existingConditionWithSameId);

                        curRule.ConditionStates[(ConditionViewModel)row.Header] = newState;
                    }
                    else if (row.Header is ActionViewModel)
                    {
                        //Remove the existing actionState first, because comparing them internally won't work right since they're different instances
                        var existingActionWithSameId = curRule.ActionStates.Keys.FirstOrDefault(cur => cur.EntityId.Equals(row.Header.EntityId));
                        if (existingActionWithSameId != null)
                            curRule.ActionStates.Remove(existingActionWithSameId);

                        curRule.ActionStates[(ActionViewModel)row.Header] = newState;
                    }
                }

                HistoryService.Instance.BeginSession();
                ViewModelService.Instance.CommitViewModel(this);
                HistoryService.Instance.EndSession();
            }
        }

    }
}
