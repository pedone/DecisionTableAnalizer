using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using HelperLibrary;

namespace DTCore
{

    internal delegate void ChangesCommittedEventHandler(Entity entity, Dictionary<string, object> oldPropertyValues, Dictionary<string, object> newPropertyValues);

    public abstract class Entity
    {

        internal event ChangesCommittedEventHandler ChangesCommitted;

        private class EntityPropertyData
        {
            private object _TemporaryValue;

            private object _Value;
            internal object Value
            {
                get { return _TemporaryValue ?? _Value; }
                set
                {
                    _TemporaryValue = value;
                    HasChanges = _TemporaryValue != ActualValue;
                }
            }

            internal object ActualValue
            {
                get { return _Value; }
            }

            internal EntityPropertyData(object value)
            {
                Value = value;
            }

            internal bool HasChanges { get; private set; }

            internal void CommitChanges()
            {
                _Value = _TemporaryValue;
                _TemporaryValue = null;
                HasChanges = false;
            }

            internal void ResetChanges()
            {
                _TemporaryValue = null;
                HasChanges = false;
            }
        }

        protected IEntityService EntityService
        {
            get { return DTCore.EntityService.Instance; }
        }

        internal EntityState State
        {
            get
            {
                if (!_IsInitialized)
                    return EntityState.New;

                return DTCore.EntityService.Instance.GetState(EntityId);
            }
        }
        public EntityId EntityId { get; private set; }
        private Dictionary<string, EntityPropertyData> PropertyValues { get; set; }

        private bool _IsInitialized;

        /// <summary>
        /// This field says if any property data has been commited before.
        /// If not, and the first validation fails, the entity will be removed again
        /// </summary>
        private bool _HasCommitedData;

        internal bool HasChanges
        {
            get { return PropertyValues.Any(cur => cur.Value.HasChanges); }
        }

        internal void SynchronizeEntityLists()
        {
            var entityListValues = PropertyValues.Where(cur => cur.Value.Value is IEntityList);
            bool wasUpdated = false;
            foreach (var listValue in entityListValues)
            {
                var entityList = (IEntityList)listValue.Value.Value;
                var deletedEntities = entityList.OfType<Entity>().Where(cur => cur.State == EntityState.Deleted);
                if (deletedEntities.Count() > 0)
                {
                    var clonedList = ListHelper.CloneList(entityList);
                    foreach (var deletedEntity in deletedEntities)
                        clonedList.Remove(deletedEntity);

                    listValue.Value.Value = clonedList;
                    wasUpdated = true;
                }
            }

            if (wasUpdated)
                CommitChanges();
        }

        protected Entity()
        {
            EntityId = new EntityId(GetType());
            PropertyValues = new Dictionary<string, EntityPropertyData>();
        }

        internal void Init()
        {
            _IsInitialized = true;
        }

        public void Delete()
        {
            DTCore.EntityService.Instance.Delete(EntityId);
        }

        public Dictionary<string, object> GetPropertyValues()
        {
            return PropertyValues.ToDictionary(cur => cur.Key, cur => cur.Value.Value);
        }

        public void SetPropertyValues(Dictionary<string, object> propertyValues)
        {
            foreach (var valuePair in propertyValues)
            {
                if (PropertyValues.ContainsKey(valuePair.Key))
                    PropertyValues[valuePair.Key].Value = valuePair.Value;
                else
                    PropertyValues[valuePair.Key] = new EntityPropertyData(valuePair.Value);
            }

            CommitChanges(true);
        }

        public void InitPropertyValues(Dictionary<string, object> propertyValues, Type[] enumTypeList)
        {
            if (propertyValues == null)
                throw new ArgumentNullException("propertyValues", "propertyValues is null.");
            if (!propertyValues.ContainsKey("EntityId"))
                throw new ArgumentException("propertyValues", "propertyValues does not contain an EntityId.");

            EntityId = new EntityId(GetType(), propertyValues["EntityId"] as string);
            var thisType = GetType();
            foreach (var value in propertyValues.Where(cur => cur.Key != "EntityId"))
            {
                var property = thisType.GetProperty(value.Key);
                object convertedValue = null;
                
                //No property by that name found
                if (property == null)
                    continue;

                if ((property.PropertyType != value.Value.GetType() && !ChangeType(value.Value, property.PropertyType, out convertedValue)))
                {
                    //see if the type is an enum
                    bool isEnumType = enumTypeList.Any(cur => cur == property.PropertyType);
                    if (!isEnumType || !Enum.IsDefined(property.PropertyType, value.Value))
                        continue;

                    convertedValue = Enum.Parse(property.PropertyType, value.Value.ToString());
                }

                PropertyValues[value.Key] = new EntityPropertyData(convertedValue ?? value.Value);
            }
        }

        internal void FireOnDelete()
        {
            OnDelete();
        }

        protected virtual void OnDelete()
        { }

        private bool ChangeType(object value, Type targetType, out object convertedValue)
        {
            try
            {
                convertedValue = Convert.ChangeType(value, targetType);
                return true;
            }
            catch
            {
                convertedValue = null;
                return false;
            }
        }

        protected TValue GetValue<TValue>(Expression<Func<TValue>> property)
        {
            var memberExpr = property.Body as MemberExpression;
            if (memberExpr == null || !(memberExpr.Member is PropertyInfo))
                throw new ArgumentException("Invalid member expression");

            PropertyInfo propertyInfo = (PropertyInfo)memberExpr.Member;
            if (PropertyValues.ContainsKey(propertyInfo.Name))
            {
                object value = PropertyValues[propertyInfo.Name].Value;
                if (value != null)
                    return (TValue)value;
                else
                    return default(TValue);
            }
            else
            {
                TValue defaultValue = default(TValue);
                PropertyValues[propertyInfo.Name] = new EntityPropertyData(defaultValue);
                return defaultValue;
            }
        }

        protected void SetValue<TValue>(Expression<Func<TValue>> property, TValue value)
        {
            var memberExpr = property.Body as MemberExpression;
            if (memberExpr == null || !(memberExpr.Member is PropertyInfo))
                throw new ArgumentException("Invalid member expression");

            PropertyInfo propertyInfo = (PropertyInfo)memberExpr.Member;
            if (PropertyValues.ContainsKey(propertyInfo.Name))
            {
                PropertyValues[propertyInfo.Name].Value = value;
            }
            else if (value != null)
            {
                PropertyValues[memberExpr.Member.Name] = new EntityPropertyData(propertyInfo)
                {
                    Value = value
                };
            }
        }

        public void CommitChanges()
        {
            CommitChanges(false);
        }

        private void CommitChanges(bool forceViewModelUpdate)
        {
            if (!ValidateInternal())
            {
                if (!_HasCommitedData)
                {
                    Delete();
                }
                else
                {
                    foreach (var data in PropertyValues.Values)
                        data.ResetChanges();
                }
            }
            else if(HasChanges)
            {
                var oldPropertyValues = PropertyValues.ToDictionary(keySelector => keySelector.Key,
                    elementSelector =>
                    {
                        if (elementSelector.Value.ActualValue is IList)
                            return ListHelper.CloneList((IList)elementSelector.Value.ActualValue);
                        else
                            return elementSelector.Value.ActualValue;
                    });

                foreach (var propertyValue in PropertyValues.Values.Where(cur => cur.HasChanges))
                    propertyValue.CommitChanges();

                var newPropertyValues = PropertyValues.ToDictionary(keySelector => keySelector.Key,
                    elementSelector =>
                    {
                        if (elementSelector.Value.ActualValue is IList)
                            return ListHelper.CloneList((IList)elementSelector.Value.ActualValue);
                        else
                            return elementSelector.Value.ActualValue;
                    });

                if (ChangesCommitted != null)
                    ChangesCommitted(this, oldPropertyValues, newPropertyValues);

                _HasCommitedData = true;
                ViewModelService.Instance.UpdateAllViewModelsWithEntity(this);
            }
            else if (forceViewModelUpdate)
            {
                _HasCommitedData = true;
                ViewModelService.Instance.UpdateAllViewModelsWithEntity(this);
            }
        }

        private bool ValidateInternal()
        {
            string error = Validate();
            if (!string.IsNullOrEmpty(error))
                StatusReporter.Instance.Add(error);

            return string.IsNullOrEmpty(error);
        }

        protected virtual string Validate()
        {
            return string.Empty;
        }

        public void ResetChanges()
        {
            foreach (var propertyValue in PropertyValues.Where(cur => cur.Value.HasChanges))
                propertyValue.Value.ResetChanges();
        }

    }
}
