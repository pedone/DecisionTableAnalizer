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
    public class DecisionTableDialogData : ViewData<DecisionTable>
    {

        public EntityId DecisionTableManagerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected override void CopyFromEntity(DecisionTable entity)
        {
            DecisionTableManagerId = entity.DecisionTableManager.EntityId;
            Name = entity.Name;
            Description = entity.Description;
        }

        protected override void CopyToEntity(DecisionTable entity)
        {
            if (entity.DecisionTableManager == null || !entity.DecisionTableManager.EntityId.Equals(DecisionTableManagerId))
            {
                if (DecisionTableManagerId != null)
                    entity.DecisionTableManager = EntityService.GetEntity<DecisionTableManager>(DecisionTableManagerId);
                else
                    entity.DecisionTableManager = null;
            }

            entity.Name = Name;
            entity.Description = Description;
        }
    }
}
