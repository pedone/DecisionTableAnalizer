using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.Windows.Input;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewEditProjectPropertiesCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            DTProject currentProject = contextViewModel.Project;
            ProjectDialogModel dialogModel = new ProjectDialogModel
            {
                Name = currentProject.Name,
                Description = currentProject.Description
            };

            ProjectDialog dialog = new ProjectDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                currentProject.Name = dialogModel.Name;
                currentProject.Description = dialogModel.Description;
            }
        }
    }
}
