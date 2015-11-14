using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(DTCondition))]
    public class SystemConditionViewData : ViewData<DTCondition>
    {

        public string Name { get; set; }
        public string Description { get; set; }

        protected override void CopyFromEntity(DTCondition entity)
        {
            Name = entity.Name;
            Description = entity.Description;
        }

        protected override void CopyToEntity(DTCondition entity)
        {
        }
    }
}
