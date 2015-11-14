using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(DTProject))]
    public class SystemProjectViewData : ViewData<DTProject>
    {

        public string Name { get; set; }

        protected override void CopyFromEntity(DTProject entity)
        {
            Name = entity.Name;
        }

        protected override void CopyToEntity(DTProject entity)
        {
        }
    }
}
