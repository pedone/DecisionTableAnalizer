using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewEditRequirementPropertiesCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemRequirementViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var dialogModel = ViewModelService.Instance.QueryViewModel<RequirementDialogModel>(contextViewModel.SelectedElement.EntityId);
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
                    ViewModelService.Instance.InsertViewModel(dialogModel, contextViewModel.RequirementManager.EntityId, ownerCollection);
                }

                HistoryService.Instance.EndSession();
            }
        }
    }
}
