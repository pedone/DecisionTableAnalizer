using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewEditProjectPropertiesCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            ProjectDialogModel dialogModel = ViewModelService.Instance.QueryViewModel<ProjectDialogModel>(contextViewModel.CurrentProject.EntityId);
            if (ViewService.Instance.ShowDialog(dialogModel) == true)
            {
                HistoryService.Instance.BeginSession();
                ViewModelService.Instance.CommitViewModel(dialogModel);
                HistoryService.Instance.EndSession();
            }
        }
    }
}
