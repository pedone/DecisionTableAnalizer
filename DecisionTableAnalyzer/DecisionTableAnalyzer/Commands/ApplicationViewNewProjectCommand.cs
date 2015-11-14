using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewNewProjectCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            ProjectDialogModel dialogModel = new ProjectDialogModel();
            ProjectDialog dialog = new ProjectDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                DTProject newProject = new DTProject
                {
                    Name = dialogModel.Name,
                    Description = dialogModel.Description
                };
                contextViewModel.SetProject(newProject);
            }
        }
    }
}
