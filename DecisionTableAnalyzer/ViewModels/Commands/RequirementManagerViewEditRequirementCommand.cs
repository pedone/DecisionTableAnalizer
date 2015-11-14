using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class RequirementManagerViewEditRequirementCommand : ViewModelCommand<RequirementManagerViewModel>
    {
        public override bool CanExecute(RequirementManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedRequirement != null;
        }

        public override void Execute(RequirementManagerViewModel contextViewModel)
        {
            var dialogModel = ViewModelService.Instance.QueryViewModel<RequirementDialogModel>(contextViewModel.SelectedRequirement.EntityId);
            var requirementKindBefore = dialogModel.Kind;
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                //TODO
                HistoryService.Instance.BeginSession();

                var requirementKindAfter = dialogModel.Kind;
                if (requirementKindBefore == requirementKindAfter)
                    ViewModelService.Instance.CommitViewModel(dialogModel);
                else
                {
                    ViewModelService.Instance.DeleteViewModel(dialogModel);

                    string ownerCollection = requirementKindAfter == DTEnums.RequirementKind.Functional ? "FunctionalRequirements" : "NonFunctionalRequirements";
                    ViewModelService.Instance.InsertViewModel(dialogModel, contextViewModel.EntityId, ownerCollection);
                }

                HistoryService.Instance.EndSession();
            }
        }
    }
}
