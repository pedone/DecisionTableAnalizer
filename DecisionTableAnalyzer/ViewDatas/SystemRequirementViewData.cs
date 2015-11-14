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
    public class SystemRequirementViewData : ViewData<Requirement>
    {

        public string Name { get; set; }
        public RequirementKind Kind { get; set; }

        protected override void CopyFromEntity(Requirement entity)
        {
            Name = entity.Name;
            Kind = entity.Kind;
        }

        protected override void CopyToEntity(Requirement entity)
        {
        }
    }
}
