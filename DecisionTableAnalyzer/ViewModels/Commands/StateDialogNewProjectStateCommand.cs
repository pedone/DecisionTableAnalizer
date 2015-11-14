using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class StateDialogNewProjectStateCommand : ViewModelCommand<StateDialogModel>
    {
        public override bool CanExecute(StateDialogModel contextViewModel)
        {
            return contextViewModel != null && !string.IsNullOrEmpty(contextViewModel.NewState.Name);
        }

        public override void Execute(StateDialogModel contextViewModel)
        {
            var oldExistingStates=contextViewModel.ExistingStates.ToList();
            var selectedStates = contextViewModel.ExistingStates.Where(cur => cur.IsSelected).ToList();

            string ownerCollection = "States";
            ViewModelService.Instance.InsertViewModel(contextViewModel.NewState, contextViewModel.DecisionTableManagerId, ownerCollection);
            contextViewModel.ResetNewStateViewModel();

            string serviceId = "DTServices.CommonServices";
            string operationId = "GetProjectStates";
            contextViewModel.ExistingStates = ViewModelService.Instance.ExecuteOperation<List<StateViewModel>>(serviceId, operationId, contextViewModel.DecisionTableManagerId);

            var newState = contextViewModel.ExistingStates.FirstOrDefault(cur => !oldExistingStates.Any(oldState => oldState.EntityId.Equals(cur.EntityId)));
            //reselect previously selected states and the new state
            foreach (var state in selectedStates.Concat(new[] { newState }))
            {
                var existingState = contextViewModel.ExistingStates.FirstOrDefault(cur => cur.EntityId.Equals(state.EntityId));
                if (existingState != null)
                    existingState.IsSelected = true;
            }
        }
    }
}
