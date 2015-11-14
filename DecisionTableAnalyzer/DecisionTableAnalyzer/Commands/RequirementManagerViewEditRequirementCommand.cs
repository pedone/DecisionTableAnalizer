using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using System.Windows;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class RequirementManagerViewEditRequirementCommand : DTCommand<RequirementManagerViewModel>
    {
        public override bool CanExecute(RequirementManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedRequirement != null;
        }

        public override void Execute(RequirementManagerViewModel contextViewModel)
        {
            Requirement selectedRequirement = contextViewModel.SelectedRequirement;

            RequirementDialogModel dialogModel = new RequirementDialogModel
            {
                Name = selectedRequirement.Name,
                Description = selectedRequirement.Description,
                Priority = selectedRequirement.Priority,
                Kind = selectedRequirement.Kind
            };
            RequirementDialog dialog = new RequirementDialog
            {
                DataContext = dialogModel,
                Owner = Application.Current.MainWindow,
            };

            if (dialog.ShowDialog() == true)
            {
                if (contextViewModel.RequirementManager.Requirements.Except(new[] { selectedRequirement }).Any(cur => cur.Name == dialogModel.Name))
                {
                    MessageBox.Show("A requirement with that name already exists.", "Error");
                }
                else
                {
                    selectedRequirement.Name = dialogModel.Name;
                    selectedRequirement.Description = dialogModel.Description;
                    selectedRequirement.Priority = dialogModel.Priority;
                    selectedRequirement.Kind = dialogModel.Kind;
                }
            }
        }
    }
}
