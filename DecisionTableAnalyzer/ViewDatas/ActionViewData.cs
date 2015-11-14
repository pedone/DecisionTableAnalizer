using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;
using DTEnums;

namespace ViewDatas
{
    [EntityType(typeof(DTAction))]
    public class ActionViewData : ViewData<DTAction>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public EntityId ReferenceSubTableId { get; set; }
        public List<StateViewData> ValidStates { get; set; }
        public StateViewData ActionEmptyState { get; set; }
        
        protected override void CopyFromEntity(DTAction entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            if (entity.ReferenceSubDecisionTable != null)
                ReferenceSubTableId = entity.ReferenceSubDecisionTable.EntityId;

            ActionEmptyState = CopyViewDataFromEntity<DTState, StateViewData>(entity.DecisionTable.DecisionTableManager.EmptyState);
            ValidStates = CopyViewDatasFromEntities<DTState, StateViewData>(entity.ValidStates);
        }

        protected override void CopyToEntity(DTAction entity)
        {
        }
    }
}
