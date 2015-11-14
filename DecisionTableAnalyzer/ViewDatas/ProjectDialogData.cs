using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using Entities;

namespace ViewDatas
{
    [EntityType(typeof(DTProject))]
    public class ProjectDialogData : ViewData<DTProject>
    {

        public string Name { get; set; }
        public string Description { get; set; }

        protected override void CopyFromEntity(DTProject entity)
        {
            Name = entity.Name;
            Description = entity.Description;
        }

        protected override void CopyToEntity(DTProject entity)
        {
            entity.Name = Name;
            entity.Description = Description;
        }
    }
}
