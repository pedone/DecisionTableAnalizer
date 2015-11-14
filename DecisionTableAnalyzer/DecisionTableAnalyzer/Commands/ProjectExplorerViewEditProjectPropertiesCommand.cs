using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Models;
using DecisionTableAnalyzer.Dialogs;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewEditProjectPropertiesCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var currentProject = contextViewModel.Project;
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
