using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewDeleteRequirementCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedItemModel is Requirement;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var selectedRequirement = contextViewModel.SelectedItemModel as Requirement;
            contextViewModel.Project.RequirementManager.Remove(selectedRequirement);
        }
    }
}
