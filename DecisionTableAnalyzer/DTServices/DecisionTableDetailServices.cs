using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels;
using Entities;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using ExtensionLibrary;

namespace DTServices
{

    public class DecisionTableDetailServices : DTService
    {

        public override object ExecuteOperation(string operationId, params object[] args)
        {
            if (operationId == "MoveConditionUp")
                return MoveConditionUp(args);
            else if (operationId == "MoveConditionDown")
                return MoveConditionDown(args);
            else if (operationId == "MoveConditionBottom")
                return MoveConditionBottom(args);
            else if (operationId == "MoveConditionTop")
                return MoveConditionTop(args);
            else if (operationId == "MoveActionUp")
                return MoveActionUp(args);
            else if (operationId == "MoveActionDown")
                return MoveActionDown(args);
            else if (operationId == "MoveActionBottom")
                return MoveActionBottom(args);
            else if (operationId == "MoveActionTop")
                return MoveActionTop(args);
            else if (operationId == "RemoveConditionState")
                return RemoveConditionState(args);
            else if (operationId == "RemoveActionState")
                return RemoveActionState(args);
            else if (operationId == "CreateSubDecisionTable")
                return CreateSubDecisionTable(args);
            else if (operationId == "DeleteSubDecisionTable")
                return DeleteSubDecisionTable(args);
            else if (operationId == "SynchronizeReferenceElementWithSubDecisionTable")
                return SynchronizeReferenceElementWithSubDecisionTable(args);
            else if (operationId == "SynchronizeSubDecisionTableWithReferenceElement")
                return SynchronizeSubDecisionTableWithReferenceElement(args);
            else
                throw new ArgumentException(string.Format("No operation found with id '{0}'.", operationId));
        }

        private object SynchronizeSubDecisionTableWithReferenceElement(object[] args)
        {
            EntityId elementId = args.FirstOrDefault() as EntityId;
            if (elementId == null ||
                (elementId.EntityType != typeof(DTAction) && elementId.EntityType != typeof(DTCondition)))
            {
                return null;
            }

            if (elementId.EntityType == typeof(DTCondition))
            {
                var condition = EntityService.GetEntity<DTCondition>(elementId);
                if (condition.ReferenceSubDecisionTable != null)
                {
                    condition.ReferenceSubDecisionTable.Name = condition.Name;
                    condition.ReferenceSubDecisionTable.Description = condition.Description;
                    condition.ReferenceSubDecisionTable.CommitChanges();
                }
            }
            else if (elementId.EntityType == typeof(DTAction))
            {
                var action = EntityService.GetEntity<DTAction>(elementId);
                if (action.ReferenceSubDecisionTable != null)
                {
                    action.ReferenceSubDecisionTable.Name = action.Name;
                    action.ReferenceSubDecisionTable.Description = action.Description;
                    action.ReferenceSubDecisionTable.CommitChanges();
                }
            }

            return null;
        }

        private object SynchronizeReferenceElementWithSubDecisionTable(object[] args)
        {
            EntityId subTableId = args.FirstOrDefault() as EntityId;
            if (subTableId == null || subTableId.EntityType != typeof(SubDecisionTable))
                return null;

            var subTable = EntityService.GetEntity<SubDecisionTable>(subTableId);
            if (subTable.ReferenceAction != null)
            {
                subTable.ReferenceAction.Name = subTable.Name;
                subTable.ReferenceAction.Description = subTable.Description;
                subTable.ReferenceAction.CommitChanges();
            }
            else if (subTable.ReferenceCondition != null)
            {
                subTable.ReferenceCondition.Name = subTable.Name;
                subTable.ReferenceCondition.Description = subTable.Description;
                subTable.ReferenceCondition.CommitChanges();
            }

            return null;
        }

        private object DeleteSubDecisionTable(object[] args)
        {
            EntityId elementId = args.FirstOrDefault() as EntityId;
            if (elementId == null ||
                (elementId.EntityType != typeof(DTAction) && elementId.EntityType != typeof(DTCondition)))
            {
                throw new ArgumentException("elementId", "elementId is invalid.");
            }

            if (elementId.EntityType == typeof(DTCondition))
            {
                var condition = EntityService.GetEntity<DTCondition>(elementId);
                if (condition.ReferenceSubDecisionTable != null)
                {
                    condition.ReferenceSubDecisionTable.Delete();
                    condition.ReferenceSubDecisionTable = null;
                }

                condition.CommitChanges();
            }
            else if (elementId.EntityType == typeof(DTAction))
            {
                var action = EntityService.GetEntity<DTAction>(elementId);
                if (action.ReferenceSubDecisionTable != null)
                {
                    action.ReferenceSubDecisionTable.Delete();
                    action.ReferenceSubDecisionTable = null;
                }

                action.CommitChanges();
            }

            return null;
        }

        private object CreateSubDecisionTable(object[] args)
        {
            EntityId elementId = args.FirstOrDefault() as EntityId;
            if (elementId == null ||
                (elementId.EntityType != typeof(DTAction) && elementId.EntityType != typeof(DTCondition)))
            {
                throw new ArgumentException("elementId", "elementId is invalid.");
            }

            var newSubTable = EntityService.CreateNew<SubDecisionTable>();
            if (elementId.EntityType == typeof(DTCondition))
            {
                var condition = EntityService.GetEntity<DTCondition>(elementId);
                condition.ReferenceSubDecisionTable = newSubTable;

                newSubTable.Name = condition.Name;
                newSubTable.DecisionTableManager = condition.DecisionTable.DecisionTableManager;
                newSubTable.ReferenceCondition = condition;

                var newSubTableList = condition.DecisionTable.SubTables.Copy();
                newSubTableList.Add(newSubTable);
                condition.DecisionTable.SubTables = newSubTableList;
            }
            else if (elementId.EntityType == typeof(DTAction))
            {
                var action = EntityService.GetEntity<DTAction>(elementId);
                action.ReferenceSubDecisionTable = newSubTable;

                newSubTable.Name = action.Name;
                newSubTable.DecisionTableManager = action.DecisionTable.DecisionTableManager;
                newSubTable.ReferenceAction = action;

                var newSubTableList = action.DecisionTable.SubTables.Copy();
                newSubTableList.Add(newSubTable);
                action.DecisionTable.SubTables = newSubTableList;
            }

            EntityService.CommitChanges();
            return null;
        }

        private object RemoveActionState(object[] args)
        {
            EntityId stateId = args.FirstOrDefault() as EntityId;
            if (stateId == null || stateId.EntityType != typeof(DTState))
                throw new ArgumentException("stateId", "stateId is invalid.");

            EntityId actionId = args.ElementAt(1) as EntityId;
            if (actionId == null || actionId.EntityType != typeof(DTAction))
                throw new ArgumentException("actionId", "actionId is invalid.");

            var action = EntityService.GetEntity<DTAction>(actionId);
            var state = EntityService.GetEntity<DTState>(stateId);

            var newValidStates = action.ValidStates.Copy();
            newValidStates.Remove(state);
            action.ValidStates = newValidStates;
            action.CommitChanges();

            return null;
        }

        private object RemoveConditionState(object[] args)
        {
            EntityId stateId = args.FirstOrDefault() as EntityId;
            if (stateId == null || stateId.EntityType != typeof(DTState))
                throw new ArgumentException("stateId", "stateId is invalid.");

            EntityId conditionId = args.ElementAt(1) as EntityId;
            if (conditionId == null || conditionId.EntityType != typeof(DTCondition))
                throw new ArgumentException("conditionId", "conditionId is invalid.");

            var condition = EntityService.GetEntity<DTCondition>(conditionId);
            var state = EntityService.GetEntity<DTState>(stateId);

            var newValidStates = condition.ValidStates.Copy();
            newValidStates.Remove(state);
            condition.ValidStates = newValidStates;
            condition.CommitChanges();

            return null;
        }

        private DTCondition GetCondition(object[] args)
        {
            EntityId conditionId = args.FirstOrDefault() as EntityId;
            if (conditionId == null || conditionId.EntityType != typeof(DTCondition))
                throw new ArgumentException("conditionId", "conditionId is invalid.");

            var condition = EntityService.GetEntity<DTCondition>(conditionId);
            if (condition == null)
                throw new ArgumentException("conditionId", "No condition found with conditionId.");

            return condition;
        }

        private object MoveConditionTop(params object[] args)
        {
            var condition = GetCondition(args);
            var newConditions = condition.DecisionTable.Conditions.Copy();
            int oldIndex = newConditions.IndexOf(condition);
            if (oldIndex > 0)
            {
                newConditions.RemoveAt(oldIndex);
                newConditions.Insert(0, condition);
                condition.DecisionTable.Conditions = newConditions;
                condition.DecisionTable.CommitChanges();
            }
            return null;
        }

        private object MoveConditionBottom(params object[] args)
        {
            var condition = GetCondition(args);
            var newConditions = condition.DecisionTable.Conditions.Copy();
            int oldIndex = newConditions.IndexOf(condition);
            if (oldIndex < (newConditions.Count - 1))
            {
                newConditions.RemoveAt(oldIndex);
                newConditions.Add(condition);
                condition.DecisionTable.Conditions = newConditions;
                condition.DecisionTable.CommitChanges();
            }
            return null;
        }

        private object MoveConditionUp(params object[] args)
        {
            var condition = GetCondition(args);
            var newConditions = condition.DecisionTable.Conditions.Copy();
            int oldIndex = newConditions.IndexOf(condition);
            if (oldIndex > 0)
            {
                newConditions.RemoveAt(oldIndex);
                newConditions.Insert(oldIndex - 1, condition);
                condition.DecisionTable.Conditions = newConditions;
                condition.DecisionTable.CommitChanges();
            }
            return null;
        }

        private object MoveConditionDown(params object[] args)
        {
            var condition = GetCondition(args);
            var newConditions = condition.DecisionTable.Conditions.Copy();
            int oldIndex = newConditions.IndexOf(condition);
            if (oldIndex < (newConditions.Count - 1))
            {
                newConditions.RemoveAt(oldIndex);
                newConditions.Insert(oldIndex + 1, condition);
                condition.DecisionTable.Conditions = newConditions;
                condition.DecisionTable.CommitChanges();
            }
            return null;
        }

        private DTAction GetAction(object[] args)
        {
            EntityId actionId = args.FirstOrDefault() as EntityId;
            if (actionId == null || actionId.EntityType != typeof(DTAction))
                throw new ArgumentException("actionId", "actionId is invalid.");

            var action = EntityService.GetEntity<DTAction>(actionId);
            if (action == null)
                throw new ArgumentException("actionId", "No action found with actionId.");

            return action;
        }

        private object MoveActionTop(params object[] args)
        {
            var action = GetAction(args);
            var newActions = action.DecisionTable.Actions.Copy();
            int oldIndex = newActions.IndexOf(action);
            if (oldIndex > 0)
            {
                newActions.RemoveAt(oldIndex);
                newActions.Insert(0, action);
                action.DecisionTable.Actions = newActions;
                action.DecisionTable.CommitChanges();
            }

            return null;
        }

        private object MoveActionBottom(params object[] args)
        {
            var action = GetAction(args);
            var newActions = action.DecisionTable.Actions.Copy();
            int oldIndex = newActions.IndexOf(action);
            if (oldIndex < (newActions.Count - 1))
            {
                newActions.RemoveAt(oldIndex);
                newActions.Add(action);
                action.DecisionTable.Actions = newActions;
                action.DecisionTable.CommitChanges();
            }

            return null;
        }

        private object MoveActionUp(params object[] args)
        {
            var action = GetAction(args);
            var newActions = action.DecisionTable.Actions.Copy();
            int oldIndex = newActions.IndexOf(action);
            if (oldIndex > 0)
            {
                newActions.RemoveAt(oldIndex);
                newActions.Insert(oldIndex - 1, action);
                action.DecisionTable.Actions = newActions;
                action.DecisionTable.CommitChanges();
            }

            return null;
        }

        private object MoveActionDown(params object[] args)
        {
            var action = GetAction(args);
            var newActions = action.DecisionTable.Actions.Copy();
            int oldIndex = newActions.IndexOf(action);
            if (oldIndex < (newActions.Count - 1))
            {
                newActions.RemoveAt(oldIndex);
                newActions.Insert(oldIndex + 1, action);
                action.DecisionTable.Actions = newActions;
                action.DecisionTable.CommitChanges();
            }

            return null;
        }

    }
}
