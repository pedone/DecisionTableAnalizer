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
    public class RemoveRedundantRulesDialogModel : ViewModel
    {

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

        public void BuildRows()
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

                rows.Add(curRow);
            }

            Rows = rows;
        }

    }
}
