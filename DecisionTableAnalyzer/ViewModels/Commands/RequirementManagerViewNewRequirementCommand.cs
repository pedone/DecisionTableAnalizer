using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;
using ViewModels;

namespace ViewModels.Commands
{
    public class RequirementManagerViewNewRequirementCommand : ViewModelCommand<RequirementManagerViewModel>
    {

        public override bool CanExecute(RequirementManagerViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(RequirementManagerViewModel contextViewModel)
        {
            RequirementDialogModel dialogModel = new RequirementDialogModel();
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();
                
                dialogModel.RequirementManagerId = contextViewModel.EntityId;
                string ownerCollection = dialogModel.Kind == DTEnums.RequirementKind.Functional ? "FunctionalRequirements" : "NonFunctionalRequirements";
                ViewModelService.Instance.InsertViewModel(dialogModel, contextViewModel.EntityId, ownerCollection);

                HistoryService.Instance.EndSession();
            }
        }

    }
}
