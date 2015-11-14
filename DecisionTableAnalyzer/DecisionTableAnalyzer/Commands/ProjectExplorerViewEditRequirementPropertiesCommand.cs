using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Models;
using DecisionTableAnalyzer.Dialogs;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewEditRequirementPropertiesCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedItemModel is Requirement;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var requirementModel = contextViewModel.SelectedItemModel as Requirement;
            RequirementDialogModel dialogModel = new RequirementDialogModel
            {
                Name = requirementModel.Name,
                Description = requirementModel.Description,
                Kind = requirementModel.Kind,
                Priority = requirementModel.Priority
            };
            RequirementDialog dialog = new RequirementDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow,
            };

            if (dialog.ShowDialog() == true)
            {
                requirementModel.Name = dialogModel.Name;
                requirementModel.Description = dialogModel.Description;
                requirementModel.Kind = dialogModel.Kind;
                requirementModel.Priority = dialogModel.Priority;
            }
        }
    }
}
