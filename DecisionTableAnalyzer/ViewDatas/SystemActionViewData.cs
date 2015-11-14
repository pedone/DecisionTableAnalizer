using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(DTAction))]
    public class SystemActionViewData : ViewData<DTAction>
    {

        public string Name { get; set; }
        public string Description { get; set; }

        protected override void CopyFromEntity(DTAction entity)
        {
            Name = entity.Name;
            Description = entity.Description;
        }

        protected override void CopyToEntity(DTAction entity)
        {
        }
    }
}
