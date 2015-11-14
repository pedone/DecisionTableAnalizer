using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace DTCore
{
    public interface IEntityService
    {
        EntityType CreateNew<EntityType>()
                where EntityType : Entity, new();
        void Insert(Entity entity);
        Entity CreateNew(Type entityType);
        EntityType GetEntity<EntityType>(EntityId entityId)
                where EntityType : Entity;
        Entity GetEntity(EntityId entityId);
        Entity GetEntity(string entityId);
        void CommitChanges();
        void ResetChanges();
        void Unload();
        IEnumerable<Entity> GetEntities(Predicate<Entity> predicate);
        Entity GetEntity(Predicate<Entity> predicate);
    }
}
