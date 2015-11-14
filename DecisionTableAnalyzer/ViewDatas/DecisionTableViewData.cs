using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;
using DTEnums;

namespace ViewDatas
{
    [EntityType(typeof(DecisionTable))]
    public class DecisionTableViewData : ViewData<DecisionTable>
    {

        public EntityId DecisionTableManagerId { get; set; }
        public string Name { get; set; }
        public List<RuleViewData> Rules { get; set; }
        public List<ActionViewData> Actions { get; set; }
        public List<ConditionViewData> Conditions { get; set; }

        protected override void CopyFromEntity(DecisionTable entity)
        {
            Name = entity.Name;
            DecisionTableManagerId = entity.DecisionTableManager.EntityId;

            Rules = CopyViewDatasFromEntities<DTRule, RuleViewData>(entity.Rules);
            Actions = CopyViewDatasFromEntities<DTAction, ActionViewData>(entity.Actions);
            Conditions = CopyViewDatasFromEntities<DTCondition, ConditionViewData>(entity.Conditions);
        }

        protected override void CopyToEntity(DecisionTable entity)
        {
            entity.Name = Name;
            entity.Rules = CopyToEntityList<RuleViewData, DTRule>(Rules);
        }
    }
}
