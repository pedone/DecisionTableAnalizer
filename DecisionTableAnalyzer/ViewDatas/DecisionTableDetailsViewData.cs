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
    public class DecisionTableDetailsViewData : ViewData<DecisionTable>
    {

        public EntityId DecisionTableManagerId { get; set; }
        public string Name { get; set; }
        public List<ConditionViewData> Conditions { get; set; }
        public List<ActionViewData> Actions { get; set; }

        protected override void CopyFromEntity(DecisionTable entity)
        {
            DecisionTableManagerId = entity.DecisionTableManager.EntityId;
            Name = entity.Name;

            Conditions = CopyViewDatasFromEntities<DTCondition, ConditionViewData>(entity.Conditions);
            Actions = CopyViewDatasFromEntities<DTAction, ActionViewData>(entity.Actions);
        }

        protected override void CopyToEntity(DecisionTable entity)
        {
            entity.Name = Name;
        }
    }
}
