using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DTCore
{
    public class HistoryService
    {

        private const int MaxSessionCount = 15;

        public static HistoryService Instance { get; private set; }
        internal HistorySession CurrentSession { get; private set; }
        private List<HistorySession> _UndoSessions = null;
        private Stack<HistorySession> _RedoSessions = null;

        public bool CanUndo
        {
            get { return _UndoSessions.Count > 0; }
        }

        public bool CanRedo
        {
            get { return _RedoSessions.Count > 0; }
        }

        static HistoryService()
        {
            Instance = new HistoryService();
        }

        private HistoryService()
        {
            _UndoSessions = new List<HistorySession>();
            _RedoSessions = new Stack<HistorySession>();
        }

        public void BeginSession()
        {
            if (CurrentSession != null)
                throw new InvalidOperationException("A session is still in progress. End the active session before beginning a new session.");

            CurrentSession = new HistorySession();
        }

        public void EndSession()
        {
            if (CurrentSession != null)
            {
                CurrentSession.Close();
                _RedoSessions.Clear();
                _UndoSessions.Add(CurrentSession);
                while (_UndoSessions.Count > MaxSessionCount)
                    _UndoSessions.RemoveAt(0);

                CurrentSession = null;
            }
        }

        public void Undo()
        {
            if (_UndoSessions.Count == 0)
                return;

            var lastSession = _UndoSessions.Last();
            _UndoSessions.Remove(lastSession);

            var redoSession = new HistorySession();
            _RedoSessions.Push(redoSession);

            while (lastSession.Entries.Count > 0)
            {
                var currentEntry = lastSession.Entries.Pop();
                if (currentEntry.Action == HistoryAction.PropertiesChanged)
                {
                    var entity = EntityService.Instance.GetEntity(currentEntry.ChangedEntityId);
                    if (entity != null)
                    {
                        redoSession.SaveOldPropertyValues(entity.EntityId, entity.GetPropertyValues());
                        entity.SetPropertyValues(currentEntry.ChangedPropertyValues);
                    }
                }
                else if (currentEntry.Action == HistoryAction.EntityDeleted)
                {
                    EntityService.Instance.ReInsert(currentEntry.DeletedEntity, redoSession);
                }
                else if (currentEntry.Action == HistoryAction.EntityInserted)
                {
                    EntityService.Instance.Delete(currentEntry.InsertedEntity, redoSession);
                }
            }

            redoSession.Close();
        }

        public void Redo()
        {
            if (_RedoSessions.Count == 0)
                return;

            var lastSession = _RedoSessions.Pop();

            var undoSession = new HistorySession();
            _UndoSessions.Add(undoSession);

            while (lastSession.Entries.Count > 0)
            {
                var currentEntry = lastSession.Entries.Pop();
                if (currentEntry.Action == HistoryAction.PropertiesChanged)
                {
                    var entity = EntityService.Instance.GetEntity(currentEntry.ChangedEntityId);
                    if (entity != null)
                    {
                        undoSession.SaveOldPropertyValues(entity.EntityId, entity.GetPropertyValues());
                        entity.SetPropertyValues(currentEntry.ChangedPropertyValues);
                    }
                }
                else if (currentEntry.Action == HistoryAction.EntityDeleted)
                {
                    EntityService.Instance.ReInsert(currentEntry.DeletedEntity, undoSession);
                }
                else if (currentEntry.Action == HistoryAction.EntityInserted)
                {
                    EntityService.Instance.ReInsert(currentEntry.InsertedEntity, undoSession);
                }
            }

            undoSession.Close();
        }

    }
}
