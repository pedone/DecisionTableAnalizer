using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class StartViewOpenRecentProjectCommand : ViewModelCommand<RecentProjectViewModel>
    {
        public override bool CanExecute(RecentProjectViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(RecentProjectViewModel contextViewModel)
        {
            if (string.IsNullOrEmpty(contextViewModel.Filename) || !File.Exists(contextViewModel.Filename))
            {
                if (MessageBox.Show(string.Format("\"{0}\" could not be opened.\nWould you like to remove the Reference Link to this project?", contextViewModel.Name),
                    "Decision Table Analyzer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    contextViewModel.ApplicationViewModel.Settings.RemoveRecentProject(contextViewModel);
                }
            }
            else
            {
                string serviceId = "DTServices.ProjectServices";
                string operationId = "LoadProject";
                ViewModelService.Instance.Unload();
                var projectId = ViewModelService.Instance.ExecuteOperation<EntityId>(serviceId, operationId, contextViewModel.Filename);
                contextViewModel.ApplicationViewModel.InitProject(projectId);
            }
        }
    }
}
