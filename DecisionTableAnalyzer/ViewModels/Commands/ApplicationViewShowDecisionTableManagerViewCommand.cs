using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewShowDecisionTableManagerViewCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded && contextViewModel.DockManager != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableManagerViewModel>())
            {
                string serviceId = "DTServices.CommonServices";
                string operationId = "GetDecisionTableManager";
                var viewModel = ViewModelService.Instance.ExecuteOperation<DecisionTableManagerViewModel>(serviceId, operationId, contextViewModel.CurrentProject.EntityId);
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
