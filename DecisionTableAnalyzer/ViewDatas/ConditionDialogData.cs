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
    public class ConditionDialogData : ViewData<DTCondition>
    {

        public EntityId DecisionTableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected override void CopyFromEntity(DTCondition entity)
        {
            DecisionTableId = entity.DecisionTable.EntityId;
            Name = entity.Name;
            Description = entity.Description;
        }

        protected override void CopyToEntity(DTCondition entity)
        {
            if (entity.DecisionTable == null || !entity.DecisionTable.EntityId.Equals(DecisionTableId))
            {
                if (DecisionTableId != null)
                    entity.DecisionTable = EntityService.GetEntity<DecisionTable>(DecisionTableId);
                else
                    entity.DecisionTable = null;
            }

            entity.Name = Name;
            entity.Description = Description;
        }
    }
}
