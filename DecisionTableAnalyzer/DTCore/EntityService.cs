using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace DTCore
{

    internal class EntityService : IEntityService
    {

        internal static EntityService Instance { get; private set; }

        private List<Entity> _InternalEntities { get; set; }
        internal ReadOnlyCollection<Entity> Entities { get; private set; }

        private bool IsHistorySessionActive
        {
            get { return CurrentHistorySession != null; }
        }

        private HistorySession CurrentHistorySession
        {
            get { return _CurrentOverrideSession ?? HistoryService.Instance.CurrentSession; }
        }

        /// <summary>
        /// This history session takes priority of the current active history session
        /// </summary>
        private HistorySession _CurrentOverrideSession;

        static EntityService()
        {
            Instance = new EntityService();
        }

        private EntityService()
        {
            _InternalEntities = new List<Entity>();
            Entities = new ReadOnlyCollection<Entity>(_InternalEntities);
        }

        public EntityType CreateNew<EntityType>()
            where EntityType : Entity, new()
        {
            var newEntity = new EntityType();
            newEntity.Init();
            _InternalEntities.Add(newEntity);
            AttachToEntity(newEntity);

            AddInsertedEntityToHistory(newEntity);
            return newEntity;
        }

        public void Insert(Entity entity)
        {
            if (entity.State != EntityState.Deleted && !_InternalEntities.Contains(entity))
            {
                entity.Init();
                _InternalEntities.Add(entity);
                AttachToEntity(entity);

                AddInsertedEntityToHistory(entity);
            }
        }

        internal void ReInsert(Entity entity, HistorySession session)
        {
            _CurrentOverrideSession = session;
            ReInsert(entity);
            _CurrentOverrideSession = null;
        }

        internal void ReInsert(Entity entity)
        {
            if (!_InternalEntities.Contains(entity))
            {
                _InternalEntities.Add(entity);
                AttachToEntity(entity);

                AddInsertedEntityToHistory(entity);
            }
        }

        internal EntityState GetState(EntityId entityId)
        {
            if (Entities.Any(cur => cur.EntityId.Equals(entityId)))
                return EntityState.Synchronized;
            else
                return EntityState.Deleted;
        }

        public Entity CreateNew(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType", "entityType is null.");
            if (!(typeof(Entity).IsAssignableFrom(entityType)))
                throw new ArgumentException("entityType", "entityType does not inherit from Entity.");

            var newEntity = Activator.CreateInstance(entityType) as Entity;
            newEntity.Init();
            _InternalEntities.Add(newEntity);
            AttachToEntity(newEntity);

            AddInsertedEntityToHistory(newEntity);
            return newEntity;
        }

        private void AttachToEntity(Entity entity)
        {
            entity.ChangesCommitted += Entity_ChangesCommitted;
        }

        private void DetachFromEntity(Entity entity)
        {
            entity.ChangesCommitted -= Entity_ChangesCommitted;
        }

        private void Entity_ChangesCommitted(Entity entity, Dictionary<string, object> oldPropertyValues, Dictionary<string, object> newPropertyValues)
        {
            if (IsHistorySessionActive)
                CurrentHistorySession.SaveOldPropertyValues(entity.EntityId, oldPropertyValues);
        }

        internal void Delete(EntityId entityId)
        {
            var entity = GetEntity(entityId);
            Delete(entity);
        }

        internal void Delete(Entity entity, HistorySession session)
        {
            _CurrentOverrideSession = session;
            Delete(entity);
            _CurrentOverrideSession = null;
        }

        /// <summary>
        /// Deletes the entity and adds the changes exclusively to the provided HistorySession
        /// </summary>
        internal void Delete(Entity entity)
        {
            if (entity == null)
                return;

            entity.FireOnDelete();
            _InternalEntities.Remove(entity);
            DetachFromEntity(entity);

            AddDeletedEntityToHistory(entity);
            SynchronizeEntities();
        }

        private void AddInsertedEntityToHistory(Entity entity)
        {
            if (IsHistorySessionActive)
                CurrentHistorySession.SaveInsertedEntity(entity);
        }

        private void AddDeletedEntityToHistory(Entity entity)
        {
            if(IsHistorySessionActive)
                CurrentHistorySession.SaveDeletedEntity(entity);
        }

        private void SynchronizeEntities()
        {
            foreach (var entity in _InternalEntities)
                entity.SynchronizeEntityLists();
        }

        public EntityType GetEntity<EntityType>(EntityId entityId)
            where EntityType : Entity
        {
            return GetEntity(entityId) as EntityType;
        }

        public Entity GetEntity(string entityId)
        {
            return Entities.FirstOrDefault(cur => cur.EntityId.Id.Equals(entityId));
        }

        public Entity GetEntity(EntityId entityId)
        {
            return Entities.FirstOrDefault(cur => cur.EntityId.Equals(entityId));
        }

        public IEnumerable<Entity> GetEntities(Predicate<Entity> predicate)
        {
            return Entities.Where(cur => predicate(cur));
        }

        public Entity GetEntity(Predicate<Entity> predicate)
        {
            return Entities.FirstOrDefault(cur => predicate(cur));
        }

        public void CommitChanges()
        {
            foreach (var entity in Entities)
            {
                if (entity.HasChanges)
                    entity.CommitChanges();
            }
        }

        /// <summary>
        /// Unload all entities
        /// </summary>
        public void Unload()
        {
            _InternalEntities.Clear();
        }

        public void ResetChanges()
        {
            foreach (var entity in Entities)
            {
                if (entity.HasChanges)
                    entity.ResetChanges();
            }
        }

    }
}
