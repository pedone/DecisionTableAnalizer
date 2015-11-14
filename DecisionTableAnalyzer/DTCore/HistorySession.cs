using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DTCore
{

    internal enum HistoryAction
    {
        PropertiesChanged,
        EntityDeleted,
        EntityInserted
    }

    internal class HistoryEntry
    {
        internal HistoryAction Action { get; private set; }
        internal Entity DeletedEntity { get; private set; }
        internal Entity InsertedEntity { get; private set; }
        internal EntityId ChangedEntityId { get; private set; }
        internal Dictionary<string, object> ChangedPropertyValues { get; private set; }

        internal void InitChangeEntry(EntityId changedEntityId, Dictionary<string, object> changedPropertyValues)
        {
            Action = HistoryAction.PropertiesChanged;
            ChangedEntityId = changedEntityId;
            ChangedPropertyValues = changedPropertyValues;
        }

        internal void InitDeleteEntry(Entity deletedEntity)
        {
            Action = HistoryAction.EntityDeleted;
            DeletedEntity = deletedEntity;
        }

        internal void InitInsertEntry(Entity insertedEntity)
        {
            Action = HistoryAction.EntityInserted;
            InsertedEntity = insertedEntity;
        }

    }

    internal class HistorySession
    {

        internal Stack<HistoryEntry> Entries { get; private set; }
        private bool _isClosed;

        internal HistorySession()
        {
            Entries = new Stack<HistoryEntry>();
        }

        internal void SaveOldPropertyValues(EntityId entityId, Dictionary<string, object> propertyValues)
        {
            if (!_isClosed)
            {
                var newEntry = new HistoryEntry();
                newEntry.InitChangeEntry(entityId, propertyValues);
                Entries.Push(newEntry);
            }
        }

        public void SaveDeletedEntity(Entity entity)
        {
            if (!_isClosed)
            {
                var newEntry = new HistoryEntry();
                newEntry.InitDeleteEntry(entity);
                Entries.Push(newEntry);
            }
        }

        public void SaveInsertedEntity(Entity entity)
        {
            if (!_isClosed)
            {
                var newEntry = new HistoryEntry();
                newEntry.InitInsertEntry(entity);
                Entries.Push(newEntry);
            }
        }

        public void Close()
        {
            _isClosed = true;

            //Remove all inserted entity changes from the changes list,
            //because it will be deleted during the next undo, so no need for changes
            var insertedEntities = from entry in Entries
                                   where entry.Action == HistoryAction.EntityInserted
                                   select entry.InsertedEntity.EntityId;
            var entriesToRemove = from entry in Entries
                                  where entry.Action == HistoryAction.PropertiesChanged
                                  where insertedEntities.Any(insertedEntityId => insertedEntityId == entry.ChangedEntityId)
                                  select entry;

            if (entriesToRemove.Count() > 0)
            {
                //Reverse the result, because the stack is backwards, the last item comes first
                IEnumerable<HistoryEntry> newEntries = Entries.Except(entriesToRemove).Reverse();
                Entries = new Stack<HistoryEntry>(newEntries);
            }
        }

    }

}
