using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewNewRequirementCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            RequirementDialogModel dialogModel = new RequirementDialogModel();
            RequirementDialog dialog = new RequirementDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow
            };
            if (dialog.ShowDialog() == true)
            {
                Requirement newRequirement = new Requirement
                {
                    Name = dialogModel.Name,
                    Description = dialogModel.Description,
                    Kind = dialogModel.Kind,
                    Priority = dialogModel.Priority
                };

                if (contextViewModel.Project.RequirementManager.Requirements.Any(cur => cur.Name == newRequirement.Name))
                    MessageBox.Show("A requirement with that name already exists.", "Error");
                else
                    contextViewModel.Project.RequirementManager.Add(newRequirement);
            }
        }
    }
}
