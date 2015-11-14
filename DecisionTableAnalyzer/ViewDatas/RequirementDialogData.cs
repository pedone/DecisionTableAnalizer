using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;
using DTEnums;

namespace ViewDatas
{
    [EntityType(typeof(Requirement))]
    public class RequirementDialogData : ViewData<Requirement>
    {

        public EntityId RequirementManagerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public RequirementKind Kind { get; set; }

        protected override void CopyFromEntity(Requirement entity)
        {
            RequirementManagerId = entity.RequirementManager.EntityId;
            Name = entity.Name;
            Description = entity.Description;
            Priority = entity.Priority;
            Kind = entity.Kind;
        }

        protected override void CopyToEntity(Requirement entity)
        {
            if (entity.RequirementManager == null || !entity.RequirementManager.EntityId.Equals(RequirementManagerId))
            {
                if (RequirementManagerId != null)
                    entity.RequirementManager = EntityService.GetEntity<RequirementManager>(RequirementManagerId);
                else
                    entity.RequirementManager = null;
            }

            entity.Name = Name;
            entity.Description = Description;
            entity.Priority = Priority;
            entity.Kind = Kind;
        }
    }
}
