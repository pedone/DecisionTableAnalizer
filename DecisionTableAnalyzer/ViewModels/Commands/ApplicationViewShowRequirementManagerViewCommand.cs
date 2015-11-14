using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewShowRequirementManagerViewCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<RequirementManagerViewModel>())
            {
                string serviceId = "DTServices.CommonServices";
                string operationId = "GetRequirementManager";
                var viewModel = ViewModelService.Instance.ExecuteOperation<RequirementManagerViewModel>(serviceId, operationId, contextViewModel.CurrentProject.EntityId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
