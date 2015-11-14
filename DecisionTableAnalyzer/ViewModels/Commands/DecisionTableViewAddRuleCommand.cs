using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using DTCore;

namespace ViewModels.Commands
{
    public class DecisionTableViewAddRuleCommand : ViewModelCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            var actionEmptyState = GetActionEmptyState(contextViewModel);
            var conditionNoPreferenceState = GetConditionNoPreferenceState(contextViewModel);

            Func<ConditionViewModel, ConditionViewModel> conditionKeySelector = condition => condition;
            Func<ConditionViewModel, StateViewModel> conditionElementSelector = condition => conditionNoPreferenceState;
            Func<ActionViewModel, ActionViewModel> actionKeySelector = action => action;
            Func<ActionViewModel, StateViewModel> actionElementSelector = action => actionEmptyState;
            var newRule = new RuleViewModel
            {
                Index = contextViewModel.Rules.Count,
                ConditionStates = contextViewModel.Conditions.ToDictionary(conditionKeySelector, conditionElementSelector),
                ActionStates = contextViewModel.Actions.ToDictionary(actionKeySelector, actionElementSelector)
            };

            HistoryService.Instance.BeginSession();
            
            string ownerCollection = "Rules";
            ViewModelService.Instance.InsertViewModel(newRule, contextViewModel.EntityId, ownerCollection);

            HistoryService.Instance.EndSession();
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

    }
}
