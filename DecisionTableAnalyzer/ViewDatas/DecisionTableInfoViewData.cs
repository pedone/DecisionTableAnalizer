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
    public class DecisionTableInfoViewData : ViewData<DecisionTable>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public int ConditionCount { get; private set; }
        public int ActionCount { get; private set; }
        public List<StateViewData> ProjectStates { get; set; }

        protected override void CopyFromEntity(DecisionTable entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            ConditionCount = entity.Conditions.Count;
            ActionCount = entity.Actions.Count;

            ProjectStates = CopyViewDatasFromEntities<DTState, StateViewData>(entity.DecisionTableManager.States);
        }

        protected override void CopyToEntity(DecisionTable entity)
        {
            entity.Name = Name;
            entity.Description = Description;
        }
    }
}
