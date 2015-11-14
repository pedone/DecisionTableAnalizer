using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCore
{
    public class EntityId
    {

        public string Id { get; private set; }
        public Type EntityType { get; private set; }

        internal EntityId(Type entityType, string id)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType", "entityType is null.");

            EntityType = entityType;
            Id = id;
        }

        internal EntityId(Type entityType)
            : this(entityType, Guid.NewGuid().ToString())
        { }

        public override bool Equals(object obj)
        {
            EntityId entityId = obj as EntityId;
            if (obj == null)
                return false;

            return Id.Equals(entityId.Id) && EntityType.Equals(entityId.EntityType);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
