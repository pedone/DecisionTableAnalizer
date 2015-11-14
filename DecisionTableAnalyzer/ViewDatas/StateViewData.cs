using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;
using DTEnums;

namespace ViewDatas
{
    [EntityType(typeof(DTState))]
    public class StateViewData : ViewData<DTState>
    {

        public string Name { get; set; }
        public string Description { get; set; }

        protected override void CopyFromEntity(DTState entity)
        {
            Name = entity.Name;
            Description = entity.Description;
        }

        protected override void CopyToEntity(DTState entity)
        {
            entity.Name = Name;
            entity.Description = Description;
        }
    }
}
