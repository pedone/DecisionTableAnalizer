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
    public class RequirementManagerViewNewRequirementCommand : DTCommand<RequirementManagerViewModel>
    {

        public override bool CanExecute(RequirementManagerViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(RequirementManagerViewModel contextViewModel)
        {
            RequirementDialogModel dialogModel = new RequirementDialogModel();
            RequirementDialog dialog = new RequirementDialog
            {
                DataContext = dialogModel,
                Owner = Application.Current.MainWindow
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

                if (contextViewModel.RequirementManager.Requirements.Any(cur => cur.Name == newRequirement.Name))
                    MessageBox.Show("A requirement with that name already exists.", "Error");
                else
                    contextViewModel.RequirementManager.Add(newRequirement);
            }
        }

    }
}
