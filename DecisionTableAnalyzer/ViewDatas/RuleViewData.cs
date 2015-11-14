using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;
using DTEnums;

namespace ViewDatas
{
    [EntityType(typeof(DTRule))]
    public class RuleViewData : ViewData<DTRule>
    {
        public Dictionary<ConditionViewData, StateViewData> ConditionStates { get; set; }
        public Dictionary<ActionViewData, StateViewData> ActionStates { get; set; }
        public int Index { get; set; }

        protected override void CopyFromEntity(DTRule entity)
        {
            Index = entity.Index;

            //Condition states
            Func<KeyValuePair<DTCondition, DTState>, ConditionViewData> conditionKeySelector =
                pair => CopyViewDataFromEntity<DTCondition, ConditionViewData>(pair.Key);
            Func<KeyValuePair<DTCondition, DTState>, StateViewData> conditionElementSelector =
                pair => CopyViewDataFromEntity<DTState, StateViewData>(pair.Value);
            ConditionStates = entity.ConditionStates.ToDictionary(conditionKeySelector, conditionElementSelector);

            //Action states
            Func<KeyValuePair<DTAction, DTState>, ActionViewData> actionKeySelector =
                pair => CopyViewDataFromEntity<DTAction, ActionViewData>(pair.Key);
            Func<KeyValuePair<DTAction, DTState>, StateViewData> actionElementSelector =
                pair => CopyViewDataFromEntity<DTState, StateViewData>(pair.Value);
            ActionStates = entity.ActionStates.ToDictionary(actionKeySelector, actionElementSelector);
        }

        protected override void CopyToEntity(DTRule entity)
        {
            entity.Index = Index;

            if (ConditionStates != null)
            {
                Func<KeyValuePair<ConditionViewData, StateViewData>, DTCondition> conditionStateKeySelector = pair =>
                {
                    var condition = EntityService.GetEntity<DTCondition>(pair.Key.EntityId);
                    pair.Key.CopyToEntity(condition);
                    return condition;
                };
                Func<KeyValuePair<ConditionViewData, StateViewData>, DTState> conditionStateElementSelector = pair =>
                {
                    var state = EntityService.GetEntity<DTState>(pair.Value.EntityId);
                    if (state != null)
                        pair.Value.CopyToEntity(state);
                    return state;
                };
                entity.ConditionStates = ConditionStates.ToDictionary(conditionStateKeySelector, conditionStateElementSelector);
            }

            if (ActionStates != null)
            {
                Func<KeyValuePair<ActionViewData, StateViewData>, DTAction> actionStateKeySelector = pair =>
                {
                    var action = EntityService.GetEntity<DTAction>(pair.Key.EntityId);
                    pair.Key.CopyToEntity(action);
                    return action;
                };
                Func<KeyValuePair<ActionViewData, StateViewData>, DTState> actionStateElementSelector = pair =>
                {
                    var state = EntityService.GetEntity<DTState>(pair.Value.EntityId);
                    if (state != null)
                        pair.Value.CopyToEntity(state);
                    return state;
                };
                entity.ActionStates = ActionStates.ToDictionary(actionStateKeySelector, actionStateElementSelector);
            }
        }
    }
}
