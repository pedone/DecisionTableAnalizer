using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCore
{
    public abstract class ViewData
    {

        protected IEntityService EntityService
        {
            get { return DTCore.EntityService.Instance; }
        }

        public EntityId EntityId { get; internal set; }

        protected ViewDataType CopyViewDataFromEntity<EntityType, ViewDataType>(EntityType entity)
            where EntityType : Entity
            where ViewDataType : ViewData
        {
            var viewData = Activator.CreateInstance<ViewDataType>();
            viewData.CopyFromEntity(entity);
            return viewData;
        }

        protected List<ViewDataType> CopyViewDatasFromEntities<EntityType, ViewDataType>(IEnumerable<EntityType> entities)
            where EntityType : Entity
            where ViewDataType : ViewData
        {
            var viewDatas = new List<ViewDataType>();
            foreach (var entity in entities)
            {
                var viewData = Activator.CreateInstance<ViewDataType>();
                viewData.CopyFromEntity(entity);
                viewDatas.Add(viewData);
            }

            return viewDatas;
        }

        abstract public void CopyFromEntity(Entity entity);
        abstract public void CopyToEntity(Entity entity);

    }

    public abstract class ViewData<EntityType> : ViewData
        where EntityType : Entity
    {

        public override void CopyFromEntity(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity", "entity is null.");
            if (!(entity is EntityType))
                throw new ArgumentException("entity", "Invalid entity type.");

            EntityId = entity.EntityId;
            CopyFromEntity((EntityType)entity);
        }

        public override void CopyToEntity(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity", "entity is null.");
            if (!(entity is EntityType))
                throw new ArgumentException("entity", "Invalid entity type.");
            //if (EntityId != null && !EntityId.Equals(entity.EntityId))
            //    throw new ArgumentException("entity", "EntityId of view data does not match id of entity.");

            CopyToEntity((EntityType)entity);
        }

        /// <summary>
        /// Inserts the viewDatas into the entity list.
        /// </summary>
        protected EntityList<EType> CopyToEntityList<VType, EType>(IEnumerable<VType> viewDatas)
            where VType : ViewData
            where EType : Entity
        {
            var entityList = new EntityList<EType>();
            foreach (var viewData in viewDatas)
            {
                var entity = viewData.EntityId != null ? EntityService.GetEntity(viewData.EntityId) : ViewModelService.Instance.CreateEntity(viewData.GetType());
                //var entity = EntityService.GetEntity(viewData.EntityId);
                if (entity != null)
                {
                    viewData.CopyToEntity(entity);
                    entity.CommitChanges();
                    if (entity.State != EntityState.Deleted)
                        entityList.Add((EType)entity);
                }
            }

            return entityList;
        }

        protected abstract void CopyFromEntity(EntityType entity);
        protected abstract void CopyToEntity(EntityType entity);

    }

}
