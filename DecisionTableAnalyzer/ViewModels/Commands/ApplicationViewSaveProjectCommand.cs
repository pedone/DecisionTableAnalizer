using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using DTCore;
using ViewModels;

namespace ViewModels.Commands
{
    public class ApplicationViewSaveProjectCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            var projectViewModel = ViewModelService.Instance.QueryViewModel<SystemProjectViewModel>(contextViewModel.CurrentProject.EntityId);
            string serviceId = "DTServices.ProjectServices";
            string operationId = "HasProjectFilename";
            bool hasFilename = ViewModelService.Instance.ExecuteOperation<bool>(serviceId, operationId, contextViewModel.CurrentProject.EntityId);

            serviceId = "DTServices.ProjectServices";
            operationId = "SaveProject";

            if (!hasFilename)
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FileName = projectViewModel.Name.Replace(' ', '_'),
                    AddExtension = true,
                    DefaultExt = "dta",
                    Filter = "DecisionTable Project|*.dta"
                };

                if (dialog.ShowDialog() == true)
                    ViewModelService.Instance.ExecuteOperation<EntityId>(serviceId, operationId, contextViewModel.CurrentProject.EntityId, dialog.FileName);
            }
            else
            {
                ViewModelService.Instance.ExecuteOperation<EntityId>(serviceId, operationId, contextViewModel.CurrentProject.EntityId);
            }
        }
    }
}
