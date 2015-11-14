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

    public class CommonServices : DTService
    {

        public override object ExecuteOperation(string operationId, params object[] args)
        {
            if (operationId == "GetRequirementManager")
                return GetRequirementManager(args);
            else if (operationId == "GetDecisionTableManager")
                return GetDecisionTableManager(args);
            else if (operationId == "GetProjectStates")
                return GetProjectStates(args);
            else if (operationId == "UnloadEntities")
                return UnloadEntities(args);
            else if (operationId == "UpdateProjectStates")
                return UpdateProjectStates(args);
            else if (operationId == "GetActionEmptyState")
                return GetActionEmptyState(args);
            else if (operationId == "GetConditionNoPreferenceState")
                return GetConditionNoPreferenceState(args);
            else
                throw new ArgumentException(string.Format("No operation found with id '{0}'.", operationId));
        }

        private StateViewModel GetConditionNoPreferenceState(params object[] args)
        {
            EntityId decisionTableManagerId = args.FirstOrDefault() as EntityId;
            if (decisionTableManagerId == null || decisionTableManagerId.EntityType != typeof(DecisionTableManager))
                throw new ArgumentException("decisionTableManagerId", "decisionTableManagerId is invalid.");

            var decisionTableManager = EntityService.GetEntity<DecisionTableManager>(decisionTableManagerId);
            return ViewModelService.Instance.QueryViewModel<StateViewModel>(decisionTableManager.NoPreferenceState.EntityId);
        }

        private StateViewModel GetActionEmptyState(params object[] args)
        {
            EntityId decisionTableManagerId = args.FirstOrDefault() as EntityId;
            if (decisionTableManagerId == null || decisionTableManagerId.EntityType != typeof(DecisionTableManager))
                throw new ArgumentException("decisionTableManagerId", "decisionTableManagerId is invalid.");

            var decisionTableManager = EntityService.GetEntity<DecisionTableManager>(decisionTableManagerId);
            return ViewModelService.Instance.QueryViewModel<StateViewModel>(decisionTableManager.EmptyState.EntityId);
        }

        private object UpdateProjectStates(params object[] args)
        {
            EntityId decisionTableManagerId = args.FirstOrDefault() as EntityId;
            if (decisionTableManagerId == null || decisionTableManagerId.EntityType != typeof(DecisionTableManager))
                throw new ArgumentException("decisionTableManagerId", "decisionTableManagerId is invalid.");

            var decisionTableManager = EntityService.GetEntity<DecisionTableManager>(decisionTableManagerId);
            var conditionsAndActions = EntityService.GetEntities(cur => cur is DTCondition || cur is DTAction);
            var conditions = conditionsAndActions.OfType<DTCondition>();
            var actions = conditionsAndActions.OfType<DTAction>();

            var unusedStates = decisionTableManager.States.Where(projectState =>
                {
                    var usedByConditions = conditions.Any(condition => condition.ValidStates.Any(usedState => usedState.EntityId.Equals(projectState.EntityId)));
                    var usedByActions = actions.Any(action => action.ValidStates.Any(usedState => usedState.EntityId.Equals(projectState.EntityId)));
                    return !usedByConditions && !usedByActions;
                }).ToList();

            foreach (var state in unusedStates)
                decisionTableManager.States.Remove(state);

            decisionTableManager.CommitChanges();
            return null;
        }

        private object UnloadEntities(params object[] args)
        {
            EntityService.Unload();
            return null;
        }

        private List<StateViewModel> GetProjectStates(params object[] args)
        {
            EntityId decisionTableManagerId = args.FirstOrDefault() as EntityId;
            if (decisionTableManagerId == null || decisionTableManagerId.EntityType != typeof(DecisionTableManager))
                throw new ArgumentException("decisionTableManagerId", "decisionTableManagerId is invalid.");

            var decisionTableManager = EntityService.GetEntity<DecisionTableManager>(decisionTableManagerId);
            return (from state in decisionTableManager.States
                    select ViewModelService.Instance.QueryViewModel<StateViewModel>(state.EntityId)).ToList();
        }
        
        private RequirementManagerViewModel GetRequirementManager(params object[] args)
        {
            EntityId projectId = args.FirstOrDefault() as EntityId;
            if (projectId == null || projectId.EntityType != typeof(DTProject))
                throw new ArgumentException("projectId", "projectId is invalid.");

            DTProject project = EntityService.GetEntity<DTProject>(projectId);
            if (project == null)
                throw new ArgumentException("projectId", "No project found with projectId.");

            return ViewModelService.Instance.QueryViewModel<RequirementManagerViewModel>(project.RequirementManager.EntityId);
        }
        
        private DecisionTableManagerViewModel GetDecisionTableManager(params object[] args)
        {
            EntityId projectId = args.FirstOrDefault() as EntityId;
            if (projectId == null || projectId.EntityType != typeof(DTProject))
                throw new ArgumentException("projectId", "projectId is invalid.");

            DTProject project = EntityService.GetEntity<DTProject>(projectId);
            if (project == null)
                throw new ArgumentException("projectId", "No project found with projectId.");

            return ViewModelService.Instance.QueryViewModel<DecisionTableManagerViewModel>(project.DecisionTableManager.EntityId);
        }
    }
}
