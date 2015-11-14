using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewSaveProjectAsCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            var projectViewModel = ViewModelService.Instance.QueryViewModel<SystemProjectViewModel>(contextViewModel.CurrentProject.EntityId);
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = projectViewModel.Name.Replace(' ', '_'),
                AddExtension = true,
                DefaultExt = "dta",
                Filter = "DecisionTable Project|*.dta"
            };

            if (dialog.ShowDialog() == true)
            {
                string serviceId = "DTServices.ProjectServices";
                string operationId = "SaveProject";
                ViewModelService.Instance.ExecuteOperation<EntityId>(serviceId, operationId, contextViewModel.CurrentProject.EntityId, dialog.FileName);
            }
        }
    }
}
