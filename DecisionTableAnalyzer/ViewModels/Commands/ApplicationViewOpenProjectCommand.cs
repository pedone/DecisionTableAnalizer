using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewOpenProjectCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "DecisionTable Project|*.dta|Xml File|*.xml|All Files|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                string serviceId = "DTServices.ProjectServices";
                string operationId = "LoadProject";
                ViewModelService.Instance.Unload();
                var projectId = ViewModelService.Instance.ExecuteOperation<EntityId>(serviceId, operationId, dialog.FileName);
                contextViewModel.InitProject(projectId);
            }
        }
    }
}
