using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(DTProject))]
    public class RecentProjectViewData : ViewData<DTProject>
    {

        public string Name { get; set; }
        public string Filename { get; set; }

        protected override void CopyFromEntity(DTProject entity)
        {
            Name = entity.Name;
            Filename = entity.Filename;
        }

        protected override void CopyToEntity(DTProject entity)
        {
        }
    }
}
