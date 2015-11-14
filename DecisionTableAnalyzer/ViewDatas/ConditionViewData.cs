using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;
using DTEnums;

namespace ViewDatas
{
    [EntityType(typeof(DTCondition))]
    public class ConditionViewData : ViewData<DTCondition>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public EntityId ReferenceSubTableId { get; set; }
        public List<StateViewData> ValidStates { get; set; }
        public StateViewData ConditionNoPreferenceState { get; set; }

        protected override void CopyFromEntity(DTCondition entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            if (entity.ReferenceSubDecisionTable != null)
                ReferenceSubTableId = entity.ReferenceSubDecisionTable.EntityId;

            ConditionNoPreferenceState = CopyViewDataFromEntity<DTState, StateViewData>(entity.DecisionTable.DecisionTableManager.NoPreferenceState);
            ValidStates = CopyViewDatasFromEntities<DTState, StateViewData>(entity.ValidStates);
        }

        protected override void CopyToEntity(DTCondition entity)
        {
        }
    }
}
