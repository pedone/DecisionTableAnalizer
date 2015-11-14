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
    public class SystemDecisionTableViewData : ViewData<DecisionTable>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public List<SystemActionViewData> Actions { get; set; }
        public List<SystemConditionViewData> Conditions { get; set; }
        public List<SystemDecisionTableViewData> SubTables { get; set; }

        protected override void CopyFromEntity(DecisionTable entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            Actions = CopyViewDatasFromEntities<DTAction, SystemActionViewData>(entity.Actions);
            Conditions = CopyViewDatasFromEntities<DTCondition, SystemConditionViewData>(entity.Conditions);
            SubTables = CopyViewDatasFromEntities<SubDecisionTable, SystemDecisionTableViewData>(entity.SubTables);
        }

        protected override void CopyToEntity(DecisionTable entity)
        {
        }
    }
}
