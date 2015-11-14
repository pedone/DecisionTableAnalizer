using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(RuleViewData))]
    public class RuleViewModel : ViewModel<RuleViewData>
    {

        private Dictionary<ConditionViewModel, StateViewModel> _ConditionStates;
        public Dictionary<ConditionViewModel, StateViewModel> ConditionStates
        {
            get { return _ConditionStates; }
            set
            {
                _ConditionStates = value;
                NotifyPropertyChanged<Dictionary<ConditionViewModel, StateViewModel>>(() => ConditionStates);
            }
        }

        private Dictionary<ActionViewModel, StateViewModel> _ActionStates;
        public Dictionary<ActionViewModel, StateViewModel> ActionStates
        {
            get { return _ActionStates; }
            set
            {
                _ActionStates = value;
                NotifyPropertyChanged<Dictionary<ActionViewModel, StateViewModel>>(() => ActionStates);
            }
        }

        private int _Index;
        public int Index
        {
            get { return _Index; }
            set
            {
                _Index = value;
                NotifyPropertyChanged<int>(() => Index);
            }
        }

        public override void CopyFromViewData(RuleViewData viewData)
        {
            Index = viewData.Index;

            //Condition states
            Func<KeyValuePair<ConditionViewData, StateViewData>, ConditionViewModel> conditionKeySelector = pair =>
                CopyViewModelFromViewData<ConditionViewData, ConditionViewModel>(pair.Key);

            Func<KeyValuePair<ConditionViewData, StateViewData>, StateViewModel> conditionElementSelector =
                pair => CopyViewModelFromViewData<StateViewData, StateViewModel>(pair.Value);
            ConditionStates = viewData.ConditionStates.ToDictionary(conditionKeySelector, conditionElementSelector);

            //Action states
            Func<KeyValuePair<ActionViewData, StateViewData>, ActionViewModel> actionKeySelector = pair =>
                CopyViewModelFromViewData<ActionViewData, ActionViewModel>(pair.Key);

            Func<KeyValuePair<ActionViewData, StateViewData>, StateViewModel> actionElementSelector =
                pair => CopyViewModelFromViewData<StateViewData, StateViewModel>(pair.Value);
            ActionStates = viewData.ActionStates.ToDictionary(actionKeySelector, actionElementSelector);
        }

        public override void CopyToViewData(RuleViewData viewData)
        {
            viewData.Index = Index;

            if (ConditionStates != null)
            {
                Func<KeyValuePair<ConditionViewModel, StateViewModel>, ConditionViewData> conditionKeySelector = pair =>
                {
                    return CreateViewData<ConditionViewData>(pair.Key);
                };
                Func<KeyValuePair<ConditionViewModel, StateViewModel>, StateViewData> conditionElementSelector = pair =>
                {
                    return CreateViewData<StateViewData>(pair.Value);
                };
                viewData.ConditionStates = ConditionStates.ToDictionary(conditionKeySelector, conditionElementSelector);
            }

            if (ActionStates != null)
            {
                Func<KeyValuePair<ActionViewModel, StateViewModel>, ActionViewData> actionKeySelector = pair =>
                {
                    return CreateViewData<ActionViewData>(pair.Key);
                };
                Func<KeyValuePair<ActionViewModel, StateViewModel>, StateViewData> actionElementSelector = pair =>
                {
                    return CreateViewData<StateViewData>(pair.Value);
                };
                viewData.ActionStates = ActionStates.ToDictionary(actionKeySelector, actionElementSelector);
            }
        }

    }
}
