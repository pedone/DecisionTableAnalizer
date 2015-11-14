using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCore
{
    public class EntityTypeAttribute : Attribute
    {
        public Type EntityType { get; set; }

        public EntityTypeAttribute(Type entityType)
        {
            EntityType = entityType;
        }
    }
}
